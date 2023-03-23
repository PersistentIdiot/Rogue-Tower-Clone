using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector2Int size = new Vector2Int(11, 11);
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private List<Vector3> path = new List<Vector3>();


    private void OnDrawGizmosSelected()
    {
        PathPoint start = GetComponentsInChildren<PathPoint>().FirstOrDefault(point => point.IsStart);
        var current = start;
        Debug.Assert(current != null);
        
        Gizmos.color = Color.blue;
        do
        {
            Gizmos.DrawLine(current.transform.position, current.NextPoint.transform.position);
            current = current.NextPoint;
        } while (current.NextPoint != null);

    }
}