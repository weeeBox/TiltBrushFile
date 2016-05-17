using System;
using System.IO;

using UnityEngine;

public static class Extensions
{
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
}

