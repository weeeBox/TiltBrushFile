using UnityEngine;

using System;
using System.Collections.Generic;

public class Edge
{
    public readonly Vector3 v1;
    public readonly Vector3 v2;

    public Edge(Vector3 v1, Vector3 v2)
    {
        this.v1 = v1;
        this.v2 = v2;
    }
}

public class TerrainMesh : MonoBehaviour
{
    [SerializeField]
    Mesh m_mesh;

    List<Edge> m_edges;

    public void GenerateGraph()
    {
        Vector3[] vertices = m_mesh.vertices;
        int[] triangles = m_mesh.triangles;

        m_edges = new List<Edge>();

        MeshGraph graph = new MeshGraph(vertices.Length);
        for (int i = 0; i < triangles.Length;)
        {
            int i1 = triangles[i++];
            int i2 = triangles[i++];
            int i3 = triangles[i++];
            Vector3 v1 = vertices[i1];
            Vector3 v2 = vertices[i2];
            Vector3 v3 = vertices[i3];

            if (graph.Add(i1, i2)) m_edges.Add(new Edge(v1, v2));
            if (graph.Add(i2, i3)) m_edges.Add(new Edge(v2, v3));
            if (graph.Add(i3, i1)) m_edges.Add(new Edge(v3, v1));
        }
    }

    void OnDrawGizmos()
    {
        if (m_edges != null)
        {
            foreach (Edge e in m_edges)
            {
                Gizmos.DrawLine(e.v1, e.v2);
            }
        }
    }
}
