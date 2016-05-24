using UnityEngine;
using System.Collections;
using System.IO;

public static class PixelReader
{
    public static Color[] ReadPixels(string path)
    {
        Texture2D texture = null;
        try
        {
            byte[] data = File.ReadAllBytes(path);
            texture = new Texture2D(1, 1);
            texture.LoadImage(data);
            return texture.GetPixels();
        }
        finally
        {
            if (texture != null)
            {
                GameObject.DestroyImmediate(texture);
            }
        }
    }
}
