using UnityEngine;
using UnityEditor;

using System.Collections;
using System.IO;
using TiltBrush;

public class MenuItems
{
    [MenuItem("Test/Read tilt")]
    static void ReadTilt()
    {
        string path = Path.Combine(new DirectoryInfo(Application.dataPath).Parent.FullName, "test.tilt");
        TiltFile tiltFile = new TiltFile(path);
        tiltFile.Write(@"C:\Users\Alex Lementuev\Documents\Tilt Brush\Sketches\test.tilt");

    }
}
