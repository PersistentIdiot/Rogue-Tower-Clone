using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Path goes from End to Start
public class Block : MonoBehaviour
{
    public Transform start;
    public List<Transform> endPoints;

    // Old
    //public Vector3 toStart { get => transform.TransformDirection(start.localPosition); }
    //public Vector3 toEnd { get => transform.TransformDirection(end.localPosition); }
}