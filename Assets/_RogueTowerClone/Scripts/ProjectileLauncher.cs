using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float attackCooldown = 1;
    
    [Header("References")]
    [SerializeField] private Projectile projectile;
    [SerializeField] private Transform spawnPosition;

    private float attackCooldownRemaining;

    private void OnEnable()
    {
        attackCooldownRemaining = attackCooldown;
    }

    private void Update()
    {
        attackCooldownRemaining -= Time.deltaTime;
    }

    public void TryFire(Enemy target)
    {
        if (target == null)
        {
            return;
        }
        if (attackCooldownRemaining <= 0)
        {
            Fire(target);
            attackCooldownRemaining = attackCooldown;
        }
    }

    private void Fire(Enemy target)
    {
        if (target == null)
        {
            Debug.Log($"Tried to fire at a null target!");
            return;
        }
        var newProjectile = Instantiate(projectile);
        newProjectile.transform.position = spawnPosition.transform.position;
        newProjectile.SetTarget(target.Model.transform);
    }
}
