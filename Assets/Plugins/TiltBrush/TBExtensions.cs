using System;
using System.IO;

using UnityEngine;
using Ionic.Zip;

namespace TiltBrushFile
{
    public static class TBExtensions
    {
        #region BinaryReader

        public static string ReadString(this BinaryReader reader, int length)
        {
            char[] chars = new char[length];
            reader.Read(chars, 0, chars.Length);
            return new string(chars, 0, chars.Length);
        }

        public static Color ReadColor(this BinaryReader reader)
        {
            float r = reader.ReadFloat();
            float g = reader.ReadFloat();
            float b = reader.ReadFloat();
            float a = reader.ReadFloat();

            return new Color(r, g, b, a);
        }

        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            float x = reader.ReadFloat();
            float y = reader.ReadFloat();
            float z = reader.ReadFloat();

            return new Vector3(x, y, z);
        }

        public static Quaternion ReadQuaternion(this BinaryReader reader)
        {
            float x = reader.ReadFloat();
            float y = reader.ReadFloat();
            float z = reader.ReadFloat();
            float w = reader.ReadFloat();

            return new Quaternion(x, y, z, w);
        }

        public static float ReadFloat(this BinaryReader reader)
        {
            return reader.ReadSingle();
        }

        public static void Skip(this BinaryReader reader, long size)
        {
            reader.BaseStream.Position += size;
        }

        #endregion

        #region BinaryWritter

        public static void Write(this BinaryWriter writter, string value, int length)
        {
            char[] chars = value.ToCharArray(0, length);
            writter.Write(chars, 0, chars.Length);
        }

        public static void Write(this BinaryWriter writter, Color value)
        {
            writter.Write(value.r);
            writter.Write(value.g);
            writter.Write(value.b);
            writter.Write(value.a);
        }

        public static void Write(this BinaryWriter writter, Vector3 value)
        {
            writter.Write(value.x);
            writter.Write(value.y);
            writter.Write(value.z);
        }

        public static void Write(this BinaryWriter writter, Quaternion value)
        {
            writter.Write(value.x);
            writter.Write(value.y);
            writter.Write(value.z);
            writter.Write(value.w);
        }

        #endregion

        #region ZipEntry

        public static string ExtractToFile(this ZipEntry entry, string baseDir)
        {
            string path = Path.Combine(baseDir, entry.FileName);
            using (FileStream stream = File.OpenWrite(path))
            {
                entry.Extract(stream);
            }
            return path;
        }

        #endregion
    }
}

