using UnityEngine;
using UnityEditor;

using System.Collections.Generic;

public class SketchAnimator
{
    IList<BrushStroke> m_strokes;
    int m_strokeIndex;
    float m_strokeDelay;
    double m_lastUpdateTime;

    public SketchAnimator(IList<BrushStroke> strokes, float strokeDelay = 0.01f)
    {
        m_strokes = strokes;
        m_strokeDelay = strokeDelay;
    }

    void Update()
    {
        if (EditorApplication.timeSinceStartup - m_lastUpdateTime > m_strokeDelay)
        {
            m_lastUpdateTime = EditorApplication.timeSinceStartup;
            if (m_strokeIndex < m_strokes.Count)
            {
                m_strokes[m_strokeIndex].gameObject.SetActive(true);
                ++m_strokeIndex;   
            }
            else
            {
                Stop();
            }
        }
    }

    public void Start()
    {
        Stop();

        m_strokeIndex = 0;
        foreach (var stroke in m_strokes)
        {
            stroke.gameObject.SetActive(false);
        }

        EditorApplication.update += Update;
    }

    public void Stop()
    {
        EditorApplication.update -= Update;
    }
}
