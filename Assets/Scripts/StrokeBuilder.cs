using UnityEngine;
using System.Collections;
using System;

public class StrokeBuilder : MonoBehaviour
{
    [SerializeField]
    FakeStroke m_fakeStroke;
    
    public FakeStroke fakeStroke
    {
        get { return m_fakeStroke; }
    }
}
