using UnityEngine;

using System;
using System.Collections;

[Serializable]
public class ControlPointData
{
    public Quaternion orientaion;
    public Vector3 position;
    public float pressure;
}

[Serializable]
public class StrokeData : ScriptableObject
{
    public Int32 brushIndex;
    public Color brushColor;
    public float brushSize;
    public ControlPointData controlPoints;
}
