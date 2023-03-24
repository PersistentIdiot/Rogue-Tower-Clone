using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Map map;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private float spawnDelay = 2;

    private float timeUntilSpawn = 0;

    private void Update()
    {
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn <= 0)
        {
            SpawnEnemies();
            timeUntilSpawn = spawnDelay;
        }
    }

    private void SpawnEnemies()
    {
        foreach (PathPoint spawnPoint in map.SpawnPoints)
        {
            var currentEnemy = Instantiate(enemyPrefab);
            currentEnemy.CurrentPoint = spawnPoint;
            currentEnemy.transform.position = spawnPoint.transform.position;
        }
    }
}
