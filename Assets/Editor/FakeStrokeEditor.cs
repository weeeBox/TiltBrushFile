using UnityEngine;
using UnityEditor;
using System.Collections;

using TiltBrush;

[CustomEditor(typeof(FakeStroke))]
public class FakeStrokeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Save"))
        {
            string folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(folderPath)) folderPath = "Assets/";

            string path = EditorUtility.SaveFilePanel("Save brush stroke", folderPath, "Brush Stroke.asset", "asset");
            if (path == "")
            {
                return;
            }

            BrushStroke stroke = (target as FakeStroke).brushStroke;

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
