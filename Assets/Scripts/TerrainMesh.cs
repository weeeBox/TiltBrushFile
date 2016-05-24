using UnityEngine;
using System.Collections;

public class TerrainMesh : MonoBehaviour
{
    [SerializeField]
    Mesh m_mesh;

    MeshGraph m_graph;

    public void GenerateGraph()
    {
        Vector3[] vertices = m_mesh.vertices;
        int[] triangles = m_mesh.triangles;

        m_graph = new MeshGraph(vertices.Length);
        for (int i = 0; i < triangles.Length;)
        {
            int i1 = triangles[i];
            int i2 = triangles[i + 1];
            int i3 = triangles[i + 2];
            Vector3 v1 = vertices[i1];
            Vector3 v2 = vertices[i2];
            Vector3 v3 = vertices[i3];

            m_graph.Add(i1, i2, v1, v2);
            m_graph.Add(i2, i3, v2, v3);
            m_graph.Add(i3, i1, v3, v1);
        }
    }

    void OnDrawGizmos()
    {
        if (m_graph != null)
        {
            bool[] visited = new bool[m_graph.Count];

        }
    }
}
