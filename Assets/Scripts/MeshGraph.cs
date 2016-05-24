using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MeshEdge
{
    public readonly int i1;
    public readonly int i2;
    public readonly Vector3 v1;
    public readonly Vector3 v2;

    public MeshEdge(int i1, int i2, Vector3 v1, Vector3 v2)
    {
        this.i1 = i1;
        this.i2 = i2;
        this.v1 = v1;
        this.v2 = v2;
    }

    public override bool Equals(object obj)
    {
        MeshEdge other = obj as MeshEdge;
        return other != null && other.i1 == i1 && other.i2 == i2;
    }
}

public class MeshGraph
{
    readonly Bag<MeshEdge>[] m_adj;

    public MeshGraph(int size)
    {
        m_adj = new Bag<MeshEdge>[size];
        for (int i = 0; i < m_adj.Length; ++i)
        {
            m_adj[i] = new Bag<MeshEdge>();
        }
    }
    
    public void Add(int i1, int i2, Vector3 v1, Vector3 v2)
    {
        Bag<MeshEdge> b1 = m_adj[i1];
        Bag<MeshEdge> b2 = m_adj[i2];

        MeshEdge edge = new MeshEdge(i1, i2, v1, v2);

        if (b1.Contains(edge) || b2.Contains(edge))
        {
            return;
        }

        b1.Add(edge);
        b2.Add(edge);
    }

    public Bag<MeshEdge> Adj(int v)
    {
        return m_adj[v];
    }

    public int Count
    {
        get { return m_adj.Length; }
    }
}

public class Bag<T> : IEnumerable<T> where T : class
{
    readonly List<T> m_list;

    public Bag()
    {
        m_list = new List<T>();
    }

    public bool Contains(T value)
    {
        return m_list.Contains(value);
    }

    public void Add(T value)
    {
        m_list.Add(value);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return m_list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return m_list.GetEnumerator();
    }
}

