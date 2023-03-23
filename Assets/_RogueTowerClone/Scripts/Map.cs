using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [FormerlySerializedAs("startingBlock")]
    [SerializeField] private Block startingBlockPrefab;
    [FormerlySerializedAs("deadEndBlock")]
    [SerializeField] private Block deadEndBlockPrefab;
    [SerializeField] private List<Block> blockPrefabs = new List<Block>();
    private List<Transform> spawnPoints = new List<Transform>();

    [Header("UI - Temporary")]
    [SerializeField] private Transform canvas;
    [SerializeField] private Button buttonPrefab;
    private List<Button> spawnTileButtons = new List<Button>();

    private List<Block> blocks = new List<Block>();
    private Block lastBlock;

    private void Start()
    {
        var startingBlock = Instantiate(startingBlockPrefab, transform);
        blocks.Add(startingBlock);
        lastBlock = startingBlock;
        spawnPoints.AddRange(startingBlock.endPoints);
        SpawnButtons();
    }


    // ToDo: Create a new path List<Vector3> for mobs to traverse from each VALID endpoint
    private void SpawnNewBlock(int endPoint = 0)
    {
        spawnPoints.Remove(lastBlock.endPoints[endPoint]);

        // Instantiate new block, add to list, name it
        var newBlock = Instantiate(blockPrefabs.GetRandomElement(), transform);
        blocks.Add(newBlock);
        newBlock.gameObject.name = $"Block {blocks.Count}";

        // Move it into the correct position
        Vector3 direction = (lastBlock.endPoints[endPoint].position - lastBlock.transform.position).normalized;
        newBlock.transform.position = lastBlock.transform.position + direction * 11;

        // ToDo: Refactor into method. DeadEndBlock will need to use it
        // Orient new block ( via Nom - https://discord.com/channels/750329891383410728/983851080255418408/1088325758050648134 )
        newBlock.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        var forwardFrom = newBlock.transform.TransformDirection(newBlock.start.localPosition);
        var forwardTo = -lastBlock.transform.TransformDirection(lastBlock.endPoints[endPoint].localPosition);
        var rotation = Quaternion.FromToRotation(forwardFrom, forwardTo);
        newBlock.transform.rotation *= rotation;

        // Orients in case it goes upside down
        float dot = Vector3.Dot(newBlock.transform.up, Vector3.up);
        if (dot <= 0)
        {
            newBlock.transform.rotation *= Quaternion.AngleAxis(180, Vector3.forward);
        }

        // Check for dead ends
        foreach (Transform newBlockEndPoint in newBlock.endPoints)
        {
            Collider[] hits = Physics.OverlapSphere(newBlockEndPoint.transform.position, 5);
            foreach (Collider hit in hits)
            {
                if (hit.TryGetComponent(out Block hitBlock))
                {
                    if (hitBlock == newBlock)
                    {
                        continue;
                    }

                    Debug.Log($"DeadEnd!");

                    var rot = newBlock.transform.rotation;
                    var pos = newBlock.transform.position;

                    DestroyImmediate(newBlock.gameObject);
                    newBlock = Instantiate(deadEndBlockPrefab, transform);
                    newBlock.transform.rotation = rot;
                    newBlock.transform.position = pos;
                    break;
                }
            }
        }

        // Add new end points
        spawnPoints.AddRange(newBlock.endPoints);

        lastBlock = newBlock;
        SpawnButtons();
    }

    private void OnDrawGizmos()
    {
        foreach (Transform endPoint in spawnPoints)
        {
            Gizmos.DrawSphere(endPoint.transform.position, 2);
        }
    }

    // ToDo: Keep a list of all valid End points, this only checks the last placed block
    private void SpawnButtons()
    {
        // Remove old buttons
        for (int i = spawnTileButtons.Count - 1; i >= 0; i--)
        {
            Destroy(spawnTileButtons[i].gameObject);
        }

        spawnTileButtons.Clear();

        for (int i = 0; i < lastBlock.endPoints.Count; i++)
        {
            // Spawn new buttons
            Block parent = spawnPoints[0].GetComponentInParent<Block>();
            Debug.Assert(parent != null);

            Vector3 direction = (lastBlock.endPoints[i].position - lastBlock.transform.position).normalized;
            var buttonPosition = lastBlock.transform.position + direction * 11 + new Vector3(0, 5, 0);

            // Check if position would result in a collision with an existing block
            var hits = Physics.RaycastAll(buttonPosition, Vector3.down, Single.PositiveInfinity);
            if (hits.Any(hit => hit.collider.TryGetComponent(out Block _)))
            {
                continue;
            }

            var newButton = Instantiate(buttonPrefab, canvas);
            spawnTileButtons.Add(newButton);
            newButton.transform.position = buttonPosition;
            int endPoint = i;
            newButton.onClick.AddListener(() => SpawnNewBlock(endPoint));
        }
    }
}