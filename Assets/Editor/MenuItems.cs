using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System.IO;
using TiltBrush;
using System;

public class MenuItems
{
    static readonly string kLastOpenDir = "lastOpenDir";
    static readonly string kLastSaveDir = "lastSaveDir";

    [MenuItem("Tilt/Export...")]
    static void ReadTilt()
    {
        string openDirectory = EditorPrefs.GetString(kLastOpenDir, Application.dataPath);

        string openPath = EditorUtility.OpenFilePanelWithFilters("Open Tilt", openDirectory, new string[] { "Tilt Files", "tilt" });
        if (string.IsNullOrEmpty(openPath)) return;

        openDirectory = new DirectoryInfo(openPath).Parent.ToString();
        EditorPrefs.SetString(kLastOpenDir, openDirectory);

        TiltFile tiltFile = new TiltFile(openPath);

        tiltFile = ProcessTilt(tiltFile);

        string saveDirectory = EditorPrefs.GetString(kLastSaveDir, openDirectory);
        string savePath = EditorUtility.SaveFilePanel("Export Tilt", saveDirectory, Path.GetFileNameWithoutExtension(openPath), "tilt");
        if (string.IsNullOrEmpty(savePath)) return;

        saveDirectory = new DirectoryInfo(savePath).Parent.ToString();
        EditorPrefs.SetString(kLastSaveDir, saveDirectory);

        tiltFile.Write(savePath);
    }

    private static TiltFile ProcessTilt(TiltFile tiltFile)
    {
        TiltFile clone = tiltFile.Clone();
        BrushStrokes strokes = clone.brushStrokes;
        float minY = 100000;

        foreach (var stroke in strokes)
        {
            foreach (var point in stroke.controlPoints)
            {
                minY = Mathf.Min(minY, point.position.y);
            }
        }

        List<BrushStroke> list = new List<BrushStroke>();

        for (int i = -1; i <= 1; ++i)
        {
            for (int j = -1; j <= 1; ++j)
            {
                foreach (var stroke in strokes)
                {
                    BrushStroke cloneStroke = stroke.Clone();
                    cloneStroke.Translate(i * 50, 0, j * 50);
                    list.Add(cloneStroke);   
                }
            }
        }

        clone.brushStrokes.Clear();
        clone.brushStrokes.AddAll(list);

        return clone;
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
