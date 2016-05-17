using UnityEngine;

using System;
using System.Collections;
using System.IO;
using Ionic.Zip;

using uint16 = System.UInt16;
using uint32 = System.UInt32;
using int32 = System.Int32;

public class TiltFile
{
    private static readonly uint SKETCH_SENTINEL = 3312887245u;
    private static readonly int SKETCH_VERSION = 5;

    public static TiltFile Read(string path)
    {
        using (FileStream stream = File.OpenRead(path))
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                return Read(reader);
            }
        }
    }

    static TiltFile Read(BinaryReader reader)
    {
        // CheckSentinel(reader);
        reader.Skip(4);

        uint16 headerSize = reader.ReadUInt16();
        uint16 headerVersion = reader.ReadUInt16();
        reader.ReadUInt32();
        reader.ReadUInt32();

        byte[] bytes = new byte[reader.BaseStream.Length - reader.BaseStream.Position];
        reader.Read(bytes, 0, bytes.Length);
        using (MemoryStream stream = new MemoryStream(bytes))
        {
            using (ZipFile file = ZipFile.Read(stream))
            {
                string tempDir = Path.GetTempPath();

                var entries = file.Entries;
                foreach (var entry in entries)
                {
                    string filename = entry.FileName;
                    if (filename == "metadata.json")
                    {
                    }
                    else if (filename == "data.sketch")
                    {   
                        entry.Extract(tempDir, ExtractExistingFileAction.OverwriteSilently);
                        string entryFile = Path.Combine(tempDir, filename);
                        ReadDataSketch(entryFile);
                    }
                }
                //var metadata = entries["metadata.json"];
                //var data = entris["data.sketch"];
                Debug.Log(entries);
            }
        }

        return null;
    }

    static void ReadDataSketch(string path)
    {
        using (FileStream stream = File.OpenRead(path))
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                CheckSentinel(reader);
                uint32 version = reader.ReadUInt32();
                uint32 reserved = reader.ReadUInt32();
                uint32 size = reader.ReadUInt32();
                reader.Skip(size);
                
                int32 num_strokes = reader.ReadInt32();
                
                for (int strokeIndex = 0; strokeIndex < num_strokes; ++strokeIndex)
                {
                    int32 brush_index = reader.ReadInt32();
                    Color brush_color = reader.ReadColor();
                    float brush_size = reader.ReadFloat();
                    reader.Skip(4); // 1u
                    reader.Skip(4); // 3u
                    uint32 strokeFlags = reader.ReadUInt32();
                    int32 num_control_points = reader.ReadInt32();
                    for (int pointIndex = 0; pointIndex < num_control_points; ++pointIndex)
                    {
                        Vector3 position = reader.ReadVector3();
                        Quaternion orientaion = reader.ReadQuaternion();
                    }
                }

                /*
                  int32 num_strokes
                  num_strokes * {
                    int32 brush_index
                    float32x4 brush_color
                    float32 brush_size
                    uint32 stroke_extension_mask
                    uint32 controlpoint_extension_mask
                    [ int32/float32              for each set bit in stroke_extension_mask &  ffff ]
                    [ uint32 size + <size> bytes for each set bit in stroke_extension_mask & ~ffff ]
                    int32 num_control_points
                    num_control_points * {
                      float32x3 position
                      float32x4 orientation (quat)
                      [ int32/float32 for each set bit in controlpoint_extension_mask ]
                    }
                  }
                */
            }
        }
    }

    static void CheckSentinel(BinaryReader reader)
    {
        uint32 sentinel = reader.ReadUInt32();
        if (sentinel != SKETCH_SENTINEL)
        {
            throw new Exception("Wrong sentinel: " + sentinel);
        }
    }
}
