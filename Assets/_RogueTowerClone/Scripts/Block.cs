using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Path goes from End to Start
public class Block : MonoBehaviour
{
    public Transform root; //ToDo: Remove after getting code working. Replace with transform
    public Transform start;
    public Transform end;

    public Vector3 toStart { get => root.TransformDirection(start.localPosition); }

    public Vector3 toEnd { get => root.TransformDirection(end.localPosition); }
}