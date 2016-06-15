using UnityEngine;
using System.Collections;
using System;

using TiltBrushFile;
using System.Collections.Generic;

public class Sketch : MonoBehaviour
{
    [SerializeField]
    BrushStroke m_fakeStroke;

    TBFile m_tileFile;

    public BrushStroke fakeStroke
    {
        get { return m_fakeStroke; }
    }

    public TBFile tiltFile
    {
        get { return m_tileFile; }
        set { m_tileFile = value; }
    }
}
