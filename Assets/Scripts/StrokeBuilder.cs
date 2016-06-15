using UnityEngine;
using System.Collections;
using System;

using TiltBrushFile;

public class StrokeBuilder : MonoBehaviour
{
    [SerializeField]
    FakeStroke m_fakeStroke;

    TBFile m_tileFile;

    public FakeStroke fakeStroke
    {
        get { return m_fakeStroke; }
    }

    public TBFile tiltFile
    {
        get { return m_tileFile; }
        set { m_tileFile = value; }
    }
}
