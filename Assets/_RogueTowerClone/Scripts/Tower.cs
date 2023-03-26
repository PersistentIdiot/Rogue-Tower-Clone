using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float towerRange = 5;
    
    [Header("References")]
    [SerializeField] private SphereCollider targetingCollider;
    [SerializeField] private GimbalController gimbalController;
    [SerializeField] private TowerTargetSelector targetSelector;
    [SerializeField] private ProjectileLauncher projectileLauncher;

    
    private Enemy targetEnemy;

    // Start is called before the first frame update
    void Start()
    {
        targetingCollider.radius = towerRange;
    }

    private void Update()
    {
        targetEnemy = targetSelector.GetClosestEnemy();
        if (targetEnemy != null)
        {
            gimbalController.TrackPosition(targetEnemy.transform.position, out float _, false);
            projectileLauncher.TryFire(targetEnemy);
        }
    }

}