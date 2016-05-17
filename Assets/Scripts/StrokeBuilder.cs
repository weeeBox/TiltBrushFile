using UnityEngine;
using System.Collections;
using System;

public class StrokeBuilder : MonoBehaviour {

    [SerializeField]
    GameObject m_controlPoint;
    
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public GameObject controlPoint
    {
        get
        {
            return m_controlPoint;
        }
    }
}
