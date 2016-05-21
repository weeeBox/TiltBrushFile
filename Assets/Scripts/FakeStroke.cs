using UnityEngine;
using System.Collections;

using TiltBrush;

public class FakeStroke : MonoBehaviour
{
    BrushStroke m_brushStroke;

    void OnDrawGizmos()
    {
        if (m_brushStroke != null)
        {
            Color color = m_brushStroke.brushColor;
            Gizmos.color = color;
            Vector3 start = m_brushStroke.controlPoints[0].position;
            for (int i = 1; i < m_brushStroke.controlPoints.Count; ++i)
            {
                Vector3 end = m_brushStroke.controlPoints[i].position;
                Gizmos.DrawLine(start, end);
                start = end;
            }
        }
    }

    public BrushStroke brushStroke
    {
        get { return m_brushStroke; }
        set { m_brushStroke = value; }
    }
}
