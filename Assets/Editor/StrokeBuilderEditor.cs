using UnityEngine;
using UnityEditor;

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
            StrokeBuilder builder = target as StrokeBuilder;
            GameObject co = builder.controlPoint;
            
            TiltFile tiltFile = TiltFile.Read(@"C:\Users\Alex Lementuev\Documents\Tilt Brush\Sketches\Untitled_9.tilt");
            foreach (var brushStroke in tiltFile.brushStrokes)
            {
                foreach (var controlPoint in brushStroke.controlPoints)
                {
                    GameObject instance = Instantiate(co, controlPoint.position, controlPoint.orientaion) as GameObject;
                    instance.GetComponentInChildren<MeshRenderer>().material.color = brushStroke.brushColor;
                    instance.name = "Control Point";
                }   
            }    
        }
    }
}
