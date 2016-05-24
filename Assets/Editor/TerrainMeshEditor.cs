using UnityEngine;
using UnityEditor;

using System.Collections;

[CustomEditor(typeof(TerrainMesh))]
public class TerrainMeshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate"))
        {
            TerrainMesh terrainMesh = target as TerrainMesh;
            terrainMesh.GenerateGraph();
        }
    }
}
