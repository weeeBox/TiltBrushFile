using UnityEngine;
using System.Collections;
using System;

using TiltBrush;

public class StrokeBuilder : MonoBehaviour
{
    [SerializeField]
    FakeStroke m_fakeStroke;

    TiltBrushFile m_tileFile;

    public FakeStroke fakeStroke
    {
        get { return m_fakeStroke; }
    }

    public TiltBrushFile tiltFile
    {
        get { return m_tileFile; }
        set { m_tileFile = value; }
    }
}
