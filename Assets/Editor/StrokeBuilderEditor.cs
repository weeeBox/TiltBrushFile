using UnityEngine;
using UnityEditor;

using System.IO;
using System.Collections;

using TiltBrush;

[CustomEditor(typeof(StrokeBuilder))]
public class StrokeBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Build"))
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
            }    
        }
    }
}
