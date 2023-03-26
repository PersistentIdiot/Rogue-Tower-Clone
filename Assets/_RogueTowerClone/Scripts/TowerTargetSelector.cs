using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerTargetSelector : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemiesInRange = new List<Enemy>();

    public Enemy GetClosestEnemy()
    {
        // Remove null entries
        enemiesInRange.RemoveAll(enemy => enemy == null);

        // Order by distance
        var closestEnemy = enemiesInRange.OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position)).FirstOrDefault();

        return closestEnemy;
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
