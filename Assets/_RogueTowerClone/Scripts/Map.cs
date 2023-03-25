using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public List<PathPoint> SpawnPoints = new List<PathPoint>();
    [SerializeField] private Block startingBlockPrefab;
    [SerializeField] private Block deadEndBlockPrefab;
    [SerializeField] private List<Block> blockPrefabs = new List<Block>();
    private List<Block> blocks = new List<Block>();

    [Header("UI - Temporary")]
    [SerializeField] private Transform canvas;
    [SerializeField] private Button buttonPrefab;
    private List<Button> spawnTileButtons = new List<Button>();

    private void Start()
    {
        var startingBlock = Instantiate(startingBlockPrefab, transform);
        blocks.Add(startingBlock);
        SpawnPoints.AddRange(startingBlock.EndPoints);
        SpawnButtons();
    }

    private void CheckForDeadEnds()
    {
        foreach (PathPoint spawnPoint in SpawnPoints)
        {
            Block spawnPointParent = spawnPoint.GetComponentInParent<Block>();
            var hits = Physics.OverlapSphere(spawnPoint.transform.position, 5);
            foreach (Collider hit in hits)
            {
                if (hit.TryGetComponent(out Block block) && block != spawnPointParent)
                {
                    // ToDo: Do something in case of dead end
                    Debug.Log($"Dead end!");
                }
            }
        }
    }

    // ToDo: Create a new path List<Vector3> for mobs to traverse from each VALID endpoint
    private void SpawnNewBlock(int spawnPoint)
    {
        // Get block this spawnPoint belongs to
        Block previousBlock = SpawnPoints[spawnPoint].GetComponentInParent<Block>();

        // Instantiate newBlock, add to list, name it
        var newBlock = Instantiate(blockPrefabs.GetRandomElement(), transform);
        blocks.Add(newBlock);
        newBlock.gameObject.name = $"Block {blocks.Count}";

        // Move newBlock into the correct position
        Vector3 direction = (SpawnPoints[spawnPoint].transform.position - previousBlock.transform.position).normalized;
        newBlock.transform.position = previousBlock.transform.position + direction * 11;

        OrientNewBlock(newBlock, spawnPoint);

        // Connect new block path to previous block
        newBlock.StartPoint.NextPoint = SpawnPoints[spawnPoint];

        // Remove old SpawnPoint, add new ones
        SpawnPoints.RemoveAt(spawnPoint);
        SpawnPoints.AddRange(newBlock.EndPoints);

        CheckForDeadEnds();
        SpawnButtons();
    }

    // Orient new block ( via Nom - https://discord.com/channels/750329891383410728/983851080255418408/1088325758050648134 )
    private void OrientNewBlock(Block newBlock, int spawnPoint)
    {
        // Get parent of spawnPoint
        Block parentBlock = SpawnPoints[spawnPoint].GetComponentInParent<Block>();

        // Calculate rotation
        newBlock.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        var forwardFrom = newBlock.transform.TransformDirection(newBlock.StartPoint.transform.localPosition);
        var forwardTo = -parentBlock.transform.TransformDirection(SpawnPoints[spawnPoint].transform.localPosition);
        var rotation = Quaternion.FromToRotation(forwardFrom, forwardTo);

        // Apply rotation
        newBlock.transform.rotation *= rotation;

        // Flips block in case it goes upside down
        float dot = Vector3.Dot(newBlock.transform.up, Vector3.up);
        if (dot <= 0)
        {
            newBlock.transform.rotation *= Quaternion.AngleAxis(180, Vector3.forward);
        }
    }

    private void SpawnButtons()
    {
        // Remove old buttons
        for (int i = spawnTileButtons.Count - 1; i >= 0; i--)
        {
            Destroy(spawnTileButtons[i].gameObject);
        }

        spawnTileButtons.Clear();

        // Spawn new buttons
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            Block parent = SpawnPoints[i].GetComponentInParent<Block>();
            Vector3 direction = (SpawnPoints[i].transform.position - parent.transform.position).normalized;
            var buttonPosition = parent.transform.position + direction * 11 + new Vector3(0, 5, 0);

            // Check if position would result in a collision with an existing block
            var hits = Physics.RaycastAll(buttonPosition, Vector3.down, Single.PositiveInfinity);
            if (hits.Any(hit => hit.collider.TryGetComponent(out Block _)))
            {
                continue;
            }

            var newButton = Instantiate(buttonPrefab, canvas);
            spawnTileButtons.Add(newButton);
            newButton.transform.position = buttonPosition;
            int spawnPoint = i;
            newButton.onClick.AddListener(() => SpawnNewBlock(spawnPoint));
        }
    }

    private void OnDrawGizmos()
    {
        foreach (PathPoint endPoint in SpawnPoints)
        {
            Gizmos.DrawSphere(endPoint.transform.position, 2);
        }
    }
}