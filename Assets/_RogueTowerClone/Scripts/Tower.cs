using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float towerRange = 5;
    [SerializeField] private SphereCollider targetingCollider;
    [SerializeField] private GimbalController gimbalController;

    private List<Enemy> enemiesInRange = new List<Enemy>();
    private Enemy closestEnemy;

    // Start is called before the first frame update
    void Start()
    {
        targetingCollider.radius = towerRange;
    }

    private void Update()
    {
        
        // Remove null entries
        enemiesInRange.RemoveAll(enemy => enemy == null);
        /*
        for (int i = enemiesInRange.Count(enemy => enemy == null) - 1; i >= 0; i--)
        {
            enemiesInRange.RemoveAt(i);
        }
        */

        // Order by distance
        closestEnemy = enemiesInRange.OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position)).FirstOrDefault();
        if (closestEnemy != null)
        {
            gimbalController.TrackPosition(closestEnemy.transform.position, out _, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            if (!enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            if (enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }
}