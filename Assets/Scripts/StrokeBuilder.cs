using UnityEngine;
using System.Collections;
using System;

using TiltBrushFile;

public class StrokeBuilder : MonoBehaviour
{
    [SerializeField]
    FakeStroke m_fakeStroke;

    TiltFile m_tileFile;

    public FakeStroke fakeStroke
    {
        get { return m_fakeStroke; }
    }

    public TiltFile tiltFile
    {
        get { return m_tileFile; }
        set { m_tileFile = value; }
    }
}
