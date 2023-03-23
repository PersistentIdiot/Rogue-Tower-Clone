using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Transform root;
    public Transform start;
    public Transform end;
    
    public Vector3 toStart {
        get => root.TransformDirection(start.localPosition);
    }
        
    public Vector3 toEnd {
        get => root.TransformDirection(end.localPosition);
    }
}