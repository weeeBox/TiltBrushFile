﻿using UnityEngine;
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
            FakeStroke template = builder.fakeStroke;

            string path = Path.Combine(new DirectoryInfo(Application.dataPath).Parent.FullName, "test.tilt");
            TiltFile tiltFile = new TiltFile(path);
            foreach (var brushStroke in tiltFile.brushStrokes)
            {
                FakeStroke stroke = Instantiate(template);
                stroke.transform.position = brushStroke.controlPoints[0].position;
                stroke.brushStroke = brushStroke;
                stroke.transform.parent = builder.transform;

                stroke.sharedMesh = CreateMesh(stroke);
                stroke.meshRenderer.material.color = stroke.brushStroke.brushColor;
                
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
        int[] triangles = new int[3 * trianglesCount];

        int vertexIndex = 0;
        foreach (var point in controlPoints)
        {
            Vector3 v1 = point.position;
            Vector3 v2 = v1 + brushStroke.brushSize * point.pressure * point.tangent;
            vertices[vertexIndex++] = v1;
            vertices[vertexIndex++] = v2;
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

        return mesh;
    }
}
