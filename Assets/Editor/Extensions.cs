using System;
using System.IO;

using UnityEngine;
using Ionic.Zip;

public static class Extensions
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

    #region ZipFile

    public static Stream OpenRead(this ZipFile zipFile, string fileName)
    {
        foreach (var entry in zipFile.Entries)
        {
            if (entry.FileName == fileName)
            {
                return OpenRead(entry);
            }
        }

        return null;
    }

    static Stream OpenRead(ZipEntry entry)
    {
        string tempDir = Path.GetTempPath();
        entry.Extract(tempDir, ExtractExistingFileAction.OverwriteSilently);
        string tempPath = Path.Combine(tempDir, entry.FileName);
        try
        {
            return new MemoryStream(File.ReadAllBytes(tempPath));
        }
        finally
        {
            if (File.Exists(tempPath)) File.Delete(tempPath);
        }
    }

    #endregion
}

