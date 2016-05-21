using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System.IO;
using TiltBrush;

public class MenuItems
{
    [MenuItem("Tilt/Export...")]
    static void ReadTilt()
    {
        string path = Path.Combine(new DirectoryInfo(Application.dataPath).Parent.FullName, "test.tilt");
        TiltFile tiltFile = new TiltFile(path);
        tiltFile.Write(@"C:\Users\Alex Lementuev\Documents\Tilt Brush\Sketches\test.tilt");
    }

    [MenuItem("Tilt/Export selected...")]
    static void ExportSelected()
    {
        IList<BrushStroke> selectedStrokes = new List<BrushStroke>();
        foreach (var selectedObject in Selection.gameObjects)
        {
            FakeStroke stroke = selectedObject.GetComponent<FakeStroke>();
            if (stroke != null && stroke.brushStroke != null)
            {
                selectedStrokes.Add(stroke.brushStroke);
            }
        }

        if (selectedStrokes.Count > 0)
        {
            StrokeBuilder builder = GameObject.FindObjectOfType<StrokeBuilder>();
            TiltFile tiltFile = builder.tiltFile;

            string path = EditorUtility.SaveFilePanel("Export Tilt", ".", "Tilt", "tilt");
            if (path.Length > 0)
            {
                TiltFile clone = tiltFile.Clone();
                clone.brushStrokes.Clear();
                clone.brushStrokes.AddAll(selectedStrokes);
                clone.Write(path);
            }
        }
    }

}
