using UnityEngine;
using UnityEditor;
using System.Collections;

using TiltBrush;

[CustomEditor(typeof(FakeStroke))]
public class FakeStrokeEditor : Editor
{
    bool m_controlPointsToggle = true;
    bool[] m_controlPointsToggles;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        FakeStroke fakeStroke = target as FakeStroke;
        BrushStroke stroke = fakeStroke.brushStroke;

        if (stroke != null)
        {
            EditorGUILayout.IntField("Index", stroke.brushIndex);
            EditorGUILayout.ColorField("Color", stroke.brushColor);
            EditorGUILayout.FloatField("Size", stroke.brushSize);
            m_controlPointsToggle = EditorGUILayout.Foldout(m_controlPointsToggle, "Control points");
            if (m_controlPointsToggle)
            {
                if (m_controlPointsToggles == null || m_controlPointsToggles.Length != stroke.controlPoints.Count)
                {
                    m_controlPointsToggles = new bool[stroke.controlPoints.Count];
                }

                int index = 0;
                foreach (var point in stroke.controlPoints)
                {
                    m_controlPointsToggles[index] = EditorGUILayout.Foldout(m_controlPointsToggles[index], "Point " + index);
                    if (m_controlPointsToggles[index])
                    {
                        EditorGUILayout.Vector3Field("Position", point.position);
                        EditorGUILayout.Vector3Field("Orientaion", point.orientaion.eulerAngles);
                        EditorGUILayout.FloatField("Pressure", point.pressure);
                        EditorGUILayout.FloatField("Timestamp", point.timestamp);
                    }

                    ++index;
                }
            }
        }


        if (GUILayout.Button("Save"))
        {
            string folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(folderPath)) folderPath = "Assets/";

            string path = EditorUtility.SaveFilePanel("Save brush stroke", folderPath, "Brush Stroke.asset", "asset");
            if (path == "")
            {
                return;
            }

            path = FileUtil.GetProjectRelativePath(path);

            StrokeData strokeData = CreateInstance<StrokeData>();
            strokeData.brushColor = stroke.brushColor;
            strokeData.brushIndex = stroke.brushIndex;
            strokeData.brushSize = stroke.brushSize;

            AssetDatabase.CreateAsset(strokeData, path);
            AssetDatabase.SaveAssets();
        }
    }
}
