using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8.5f; // Speed of projectile.
    public float radius = 1f; // Collision radius.
    float radiusSq; // Radius squared; optimisation.
    Transform target; // Who we are homing at.

    void OnEnable()
    {
        // Pre-compute the value. 
        radiusSq = radius * radius;
    }

    void Update()
    {
        // If there is no target, destroy itself and end execution.
        if (!target)
        {
            Destroy(gameObject);
            // Write your own code to spawn an explosion / splat effect.
            return; // Stops executing this function.
        }

        // Move ourselves towards the target at every frame.
        Vector3 direction = target.position - transform.position;
        transform.position += direction.normalized * speed * Time.deltaTime;

        // Destroy the projectile if it is close to the target.
        if (direction.sqrMagnitude < radiusSq)
        {
            Destroy(gameObject);
            Destroy(target.GetComponentInParent<Enemy>().gameObject);
            // Write your own code to spawn an explosion / splat effect.
            // Write your own code to deal damage to the <target>.
        }
    }

    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }
}