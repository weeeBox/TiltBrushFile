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
        static readonly string kFileSketchData  = "data.sketch";
        static readonly string kFileMetadata    = "metadata.json";
        static readonly string kFileThumbnail   = "thumbnail.png";

        private static readonly uint SKETCH_SENTINEL = 3312887245u;
        private static readonly int SKETCH_VERSION = 5;

        TiltHeader m_header;
        BrushStrokes m_brushStrokes;
        string m_metadata;
        byte[] m_thumbnailBytes;

        public TiltFile(string path)
        {
            using (FileStream stream = File.OpenRead(path))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    Read(reader);
                }
            }
        }

        public TiltFile(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                Read(reader);
            }
        }

        void Read(BinaryReader reader)
        {
            m_header = new TiltHeader(reader);

            byte[] bytes = new byte[reader.BaseStream.Length - reader.BaseStream.Position];
            reader.Read(bytes, 0, bytes.Length);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (ZipFile zipFile = ZipFile.Read(stream))
                {
                    string tempDir = GetTempDirectory(".tilt-in");
                    foreach (var entry in zipFile.Entries)
                    {
                        entry.ExtractToFile(tempDir);
                    }

                    try
                    {
                        m_brushStrokes = ReadBrushStrokes(Path.Combine(tempDir, kFileSketchData));
                        m_metadata = ReadMetadata(Path.Combine(tempDir, kFileMetadata));
                        m_thumbnailBytes = ReadThumbnailBytes(Path.Combine(tempDir, kFileThumbnail));
                    }
                    finally
                    {
                        Directory.Delete(tempDir, true);
                    }
                }
            }
        }

        #region Read

        static BrushStrokes ReadBrushStrokes(string path)
        {
            using (Stream stream = File.OpenRead(path))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    return new BrushStrokes(reader);
                }
            }
        }

        static string ReadMetadata(string path)
        {
            return File.ReadAllText(path);
        }

        static byte[] ReadThumbnailBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        #endregion

        #region Write

        public void Write(string path)
        {
            string tempDir = GetTempDirectory(".tilt-out");
            try
            {
                WriteToTempDir(tempDir);

                using (FileStream stream = File.OpenWrite(path))
                {
                    using (BinaryWriter writter = new BinaryWriter(stream))
                    {
                        m_header.Write(writter);

                        using (ZipFile zipFile = new ZipFile())
                        {
                            foreach (string file in Directory.GetFiles(tempDir))
                            {
                                zipFile.AddFile(file);
                            }
                            zipFile.Save(writter.BaseStream);
                        }
                    }
                }
            }
            finally
            {
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
            }
        }

        void WriteToTempDir(string tempDir)
        {
            string sketchFile  = Path.Combine(tempDir, "data.sketch");
            using (FileStream stream = File.OpenWrite(sketchFile))
            {
                using (BinaryWriter writter = new BinaryWriter(stream))
                {
                    m_brushStrokes.Write(writter);
                }
            }

            string metadataFile = Path.Combine(tempDir, "metadata.json");
            File.WriteAllText(metadataFile, m_metadata);

            string thumbnailFile = Path.Combine(tempDir, "thumbnail.png");
            File.WriteAllBytes(thumbnailFile, m_thumbnailBytes);
        }

        #endregion

        #region Helpers

        static string GetTempDirectory(string name, bool createIsNotExists = true)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), name);
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
            if (createIsNotExists)
            {
                Directory.CreateDirectory(tempDir);
            }

            return tempDir;
        }

        #endregion

        public BrushStrokes brushStrokes
        {
            get { return m_brushStrokes; }
        }
    }
}
