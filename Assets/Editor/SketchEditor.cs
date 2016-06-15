using UnityEngine;
using UnityEditor;

using System.IO;
using System.Collections;
using System.Collections.Generic;

using TiltBrushFile;
using System;

[CustomEditor(typeof(Sketch))]
public class SketchEditor : Editor
{
    static readonly string kLastTiltFileDirectory = "LastTiltFileDirectory";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Load tilt file"))
        {
            string tiltPath = ResolveTiltFileDirectory();
            string path = EditorUtility.OpenFilePanel("Open sketch", tiltPath, "tilt");
            if (string.IsNullOrEmpty(path)) return;

            EditorPrefs.SetString(kLastTiltFileDirectory, Directory.GetParent(path).ToString());

            Sketch sketch = target as Sketch;
            foreach (var s in sketch.GetComponentsInChildren<BrushStroke>())
            {
                DestroyImmediate(s.gameObject);
            }

            BrushStroke bruskStrokeTemplate = sketch.fakeStroke;
            TBFile tiltFile = new TBFile(path);
            foreach (var brushStroke in tiltFile.brushStrokes)
            {
                BrushStroke stroke = Instantiate(bruskStrokeTemplate);
                stroke.transform.position = brushStroke.startPosition;
                stroke.transform.parent = sketch.transform;

                stroke.sharedMesh = CreateMesh(stroke, brushStroke);
            }
            sketch.tiltFile = tiltFile;  
        }
        
        if (GUILayout.Button("Animate"))
        {
            Sketch sketch = target as Sketch;
            new SketchAnimator(sketch.GetComponentsInChildren<BrushStroke>()).Start();
        }        
    }

    private Mesh CreateMesh(BrushStroke stroke, TBBrushStroke brushStroke)
    {
        var controlPoints = brushStroke.controlPoints;

        Mesh mesh = new Mesh();
        int vertexCount = 2 * 2 * controlPoints.Count; // triangle strip is double-sided
        int trianglesCount = 2 * (2 * controlPoints.Count - 2);

        Vector3[] vertices = new Vector3[vertexCount];
        Color[] colors = new Color[vertexCount];
        int[] triangles = new int[3 * trianglesCount];

        int vertexIndex = 0;

        // generate front side
        foreach (var point in controlPoints)
        {
            Vector3 v1 = point.position - stroke.transform.position;
            Vector3 v2 = v1 + brushStroke.brushSize * point.pressure * point.tangent;
            vertices[vertexIndex++] = v1;
            vertices[vertexIndex++] = v2;
        }
        // generate back side
        Array.Copy(vertices, 0, vertices, vertices.Length / 2, vertices.Length / 2);

        // set colors
        for (int i = 0; i < vertexCount; ++i)
        {
            colors[i] = brushStroke.brushColor;
        }

        // generate triangles
        vertexIndex = 0;
        for (int i = 0; i < triangles.Length / 2;)
        {
            int i1 = vertexIndex;
            int i2 = vertexIndex + 1;
            int i3 = vertexIndex + 2;
            int i4 = vertexIndex + 3;

            triangles[i++] = i1;
            triangles[i++] = i2;
            triangles[i++] = i3;
            triangles[i++] = i3;
            triangles[i++] = i2;
            triangles[i++] = i4;

            vertexIndex += 2;
        }

        for (int i = triangles.Length / 2; i < triangles.Length;)
        {
            int i1 = vertexIndex;
            int i2 = vertexIndex + 1;
            int i3 = vertexIndex + 2;
            int i4 = vertexIndex + 3;

            triangles[i++] = i1;
            triangles[i++] = i3;
            triangles[i++] = i2;
            triangles[i++] = i3;
            triangles[i++] = i4;
            triangles[i++] = i2;

            vertexIndex += 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();

        return mesh;
    }

    #region Helpers

    static string ResolveTiltFileDirectory()
    {
        string path = EditorPrefs.GetString(kLastTiltFileDirectory);
        if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
        {
            return path;
        }

        path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Path.Combine("Tilt Brush", "Sketches"));
        if (Directory.Exists(path))
        {
            return path;
        }

        return Application.dataPath;
    }

    #endregion
}
