using UnityEngine;
using System.Collections;

using TiltBrushFile;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class FakeStroke : MonoBehaviour
{
    TBBrushStroke m_brushStroke;
    MeshFilter m_meshFilter;
    MeshRenderer m_meshRenderer;

    void Awake()
    {
        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();
    }

    //void OnDrawGizmos()
    //{
    //    DrawStroke(false);
    //}

    //void OnDrawGizmosSelected()
    //{
    //    DrawStroke(true);
    //}

    void DrawStroke(bool selected)
    {
        if (m_brushStroke != null)
        {
            Color color = selected ? Color.white : m_brushStroke.brushColor;
            Gizmos.color = color;
            Vector3 start = m_brushStroke.controlPoints[0].position;
            for (int i = 1; i < m_brushStroke.controlPoints.Count; ++i)
            {
                Vector3 end = m_brushStroke.controlPoints[i].position;
                Gizmos.DrawLine(start, end);
                start = end;
            }

            //foreach (var point in m_brushStroke.controlPoints)
            //{
            //    // Gizmos.DrawLine(point.position, point.position + point.orientaion * Vector3.forward);
            //    Gizmos.DrawLine(point.position, point.position + m_brushStroke.brushSize * point.pressure * (point.orientaion * Vector3.up));
            //}
        }
    }

    public TBBrushStroke brushStroke
    {
        get { return m_brushStroke; }
        set { m_brushStroke = value; }
    }
    
    public Mesh sharedMesh
    {
        get { return meshFilter.sharedMesh; }
        set { meshFilter.sharedMesh = value; }
    }

    public MeshFilter meshFilter
    {
        get
        {
            if (m_meshFilter == null)
            {
                m_meshFilter = GetComponent<MeshFilter>();
            }
            return m_meshFilter;
        }
    }

    public MeshRenderer meshRenderer
    {
        get
        {
            if (m_meshRenderer == null)
            {
                m_meshRenderer = GetComponent<MeshRenderer>();
            }
            return m_meshRenderer;
        }
    }
}
