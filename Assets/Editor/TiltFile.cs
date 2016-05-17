using UnityEngine;

using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;

using uint16 = System.UInt16;
using uint32 = System.UInt32;
using int32 = System.Int32;

namespace TiltBrush
{
    public class TiltFile
    {
        private static readonly uint SKETCH_SENTINEL = 3312887245u;
        private static readonly int SKETCH_VERSION = 5;

        private List<BrushStroke> m_brushStrokes;
        private string m_metadata;
        private byte[] m_thumbnailBytes;

        public TiltFile(List<BrushStroke> brushStrokes, string metadata, byte[] previewBytes)
        {
            m_brushStrokes = brushStrokes;
            m_metadata = metadata;
            m_thumbnailBytes = previewBytes;
        }

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
            string sentinel = reader.ReadString(4);
            if (sentinel != "tilT")
            {
                throw new Exception("Wrong sentinel: " + sentinel);
            }

            uint16 headerSize = reader.ReadUInt16();
            uint16 headerVersion = reader.ReadUInt16();
            reader.ReadUInt32();
            reader.ReadUInt32();

            byte[] bytes = new byte[reader.BaseStream.Length - reader.BaseStream.Position];
            reader.Read(bytes, 0, bytes.Length);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (ZipFile zipFile = ZipFile.Read(stream))
                {
                    List<BrushStroke> brushStrokes = ReadBrushStrokes(zipFile);
                    string metadata = ReadMetadata(zipFile);
                    byte[] thumbnailBytes = ReadThumbnailBytes(zipFile);

                    return new TiltFile(brushStrokes, metadata, thumbnailBytes);
                }
            }
        }
        
        private static List<BrushStroke> ReadBrushStrokes(ZipFile zipFile)
        {
            using (Stream stream = zipFile.OpenRead("data.sketch"))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    uint32 sentinel = reader.ReadUInt32();
                    if (sentinel != SKETCH_SENTINEL)
                    {
                        throw new Exception("Wrong sentinel: " + sentinel);
                    }

                    uint32 version = reader.ReadUInt32();
                    uint32 reserved = reader.ReadUInt32();
                    uint32 size = reader.ReadUInt32();
                    reader.Skip(size);

                    int32 strokeCount = reader.ReadInt32();
                    List<BrushStroke> brushStrokes = new List<BrushStroke>(strokeCount);
                    for (int strokeIndex = 0; strokeIndex < strokeCount; ++strokeIndex)
                    {
                        brushStrokes.Add(BrushStroke.Read(reader));
                    }

                    return brushStrokes;
                }
            }
        }

        private static string ReadMetadata(ZipFile zipFile)
        {
            return zipFile.ReadAllText("metadata.json");
        }

        private static byte[] ReadThumbnailBytes(ZipFile zipFile)
        {
            return zipFile.ReadAllBytes("thumbnail.png");
        }

        public void Write(string path)
        {   
        }

        public List<BrushStroke> brushStrokes
        {
            get { return m_brushStrokes; }
        }
    }
}
