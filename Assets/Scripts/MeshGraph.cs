using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MeshGraph
{
    readonly Bag<int>[] m_adj;

    public MeshGraph(int size)
    {
        m_adj = new Bag<int>[size];
        for (int i = 0; i < m_adj.Length; ++i)
        {
            m_adj[i] = new Bag<int>();
        }
    }
    
    public bool Add(int i1, int i2)
    {
        Bag<int> b1 = m_adj[i1];
        Bag<int> b2 = m_adj[i2];

        if (b1.Contains(i1) || b2.Contains(i2))
        {
            return false;
        }

        b1.Add(i2);
        b2.Add(i1);

        return true;
    }

    public Bag<int> Adj(int v)
    {
        return m_adj[v];
    }

    public int Count
    {
        get { return m_adj.Length; }
    }
}

public class Bag<T> : IEnumerable<T>
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

