using UnityEngine;
using UnityEditor;

using System.IO;
using System.Collections;
using System.Collections.Generic;

using TiltBrush;
using System;

[CustomEditor(typeof(StrokeBuilder))]
public class StrokeBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Load"))
        {
            foreach (var s in GameObject.FindObjectsOfType<FakeStroke>())
            {
                DestroyImmediate(s.gameObject);
            }

            StrokeBuilder builder = target as StrokeBuilder;
            FakeStroke fakeStrokeTemplate = builder.fakeStroke;

            string path = Path.Combine(new DirectoryInfo(Application.dataPath).Parent.FullName, "test.tilt");
            TiltFile tiltFile = new TiltFile(path);
            foreach (var brushStroke in tiltFile.brushStrokes)
            {
                FakeStroke fakeStroke = Instantiate(fakeStrokeTemplate);
                fakeStroke.transform.position = brushStroke.startPosition;
                fakeStroke.brushStroke = brushStroke;
                fakeStroke.transform.parent = builder.transform;

                fakeStroke.sharedMesh = CreateMesh(fakeStroke);
            }
            builder.tiltFile = tiltFile;  
        }        
    }

    private Mesh CreateMesh(FakeStroke stroke)
    {
        BrushStroke brushStroke = stroke.brushStroke;
        var controlPoints = brushStroke.controlPoints;

        Mesh mesh = new Mesh();
        int vertexCount = 2 * controlPoints.Count;
        int trianglesCount = vertexCount - 2;

        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] normals = new Vector3[vertexCount];
        Color[] colors = new Color[vertexCount];
        int[] triangles = new int[3 * trianglesCount];

        int vertexIndex = 0;
        foreach (var point in controlPoints)
        {
            Vector3 v1 = point.position;
            Vector3 v2 = v1 + brushStroke.brushSize * point.pressure * point.tangent;
            int i1 = vertexIndex++;
            int i2 = vertexIndex++;
            vertices[i1] = v1;
            vertices[i2] = v2;
            colors[i1] = colors[i2] = brushStroke.brushColor;
        }

        vertexIndex = 0;
        for (int i = 0; i < triangles.Length;)
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

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        return mesh;
    }
}
