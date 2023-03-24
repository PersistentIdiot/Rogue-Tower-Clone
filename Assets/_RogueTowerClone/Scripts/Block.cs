using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Path goes from End to Start
public class Block : MonoBehaviour
{
    public PathPoint Start;
    public List<PathPoint> EndPoints;
}