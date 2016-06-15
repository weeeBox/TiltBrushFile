using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class BrushStroke : MonoBehaviour
{
    MeshFilter m_meshFilter;
    MeshRenderer m_meshRenderer;

    void Awake()
    {
        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();
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
