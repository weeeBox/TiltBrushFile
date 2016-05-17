using UnityEngine;
using UnityEditor;

using System.Collections;

public class TiltReader
{
    [MenuItem("Test/Read tilt")]
    static void ReadTilt()
    {
        TiltFile.Read("c:\\Users\\Alex Lementuev\\Documents\\Untitled_27.tilt");
    }
}
