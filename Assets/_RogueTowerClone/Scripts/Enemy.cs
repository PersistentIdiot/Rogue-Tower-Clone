using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float pointDistance = 0.1f;
    public GameObject Model;
    public PathPoint CurrentPoint;

    private void Update()
    {
        if (Vector3.Distance(transform.position, CurrentPoint.transform.position) <= pointDistance)
        {
            transform.position = CurrentPoint.transform.position;
            CurrentPoint = CurrentPoint.NextPoint;
        }

        if (CurrentPoint == null)
        {
            ReachGoal();
            return;
        }

        var direction = (CurrentPoint.transform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void ReachGoal()
    {
        Destroy(gameObject);
    }
}