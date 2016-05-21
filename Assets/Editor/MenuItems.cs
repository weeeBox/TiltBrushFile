using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System.IO;
using TiltBrush;
using System;

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
            selectedStrokes = AlignStokes(selectedStrokes);

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

    private static IList<BrushStroke> AlignStokes(IList<BrushStroke> selectedStrokes)
    {
        Vector3 min = new Vector3(1000, 1000, 1000);
        Vector3 max = new Vector3(-1000, -1000, -1000);

        foreach (var stroke in selectedStrokes)
        {
            foreach (var point in stroke.controlPoints)
            {
                min.x = Mathf.Min(min.x, point.position.x);
                min.z = Mathf.Min(min.z, point.position.z);

                max.x = Mathf.Max(max.x, point.position.x);
                max.z = Mathf.Max(max.z, point.position.z);
            }
        }

        Vector3 offset = min + 0.5f * (max - min);
        Debug.Log("Center: " + offset);

        IList<BrushStroke> strokes = new List<BrushStroke>(selectedStrokes.Count);
        foreach (var stroke in selectedStrokes)
        {
            var clone = stroke.Clone();
            foreach (var point in clone.controlPoints)
            {
                point.position -= offset;            
            }
            strokes.Add(clone);
        }

        return strokes;     
    }
}
