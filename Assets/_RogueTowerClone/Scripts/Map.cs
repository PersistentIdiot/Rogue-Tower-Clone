using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nomnom.RaycastVisualization;
using Palmmedia.ReportGenerator.Core.Logging;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [SerializeField] private Block startingBlock;
    [SerializeField] private Block deadEndBlock;
    [SerializeField] private List<Block> blockPrefabs = new List<Block>();

    [Header("UI - Temporary")]
    [SerializeField] private Transform canvas;
    [SerializeField] private Button buttonPrefab;
    private List<Button> spawnTileButtons = new List<Button>();

    private List<Block> blocks = new List<Block>();
    private Block lastBlock;

    private void Start()
    {
        blocks.Add(Instantiate(startingBlock, transform));
        lastBlock = startingBlock;
        SpawnButtons();
    }


    // ToDo: Check if start intersects previous path and replace with a new EndBlock prefab
    // ToDo: Create a new path List<Vector3> for mobs to traverse from each VALID endpoint
    public void SpawnNewBlock(int endPoint = 0)
    {
        // Instantiate new block, add to list, name it
        var newBlock = Instantiate(blockPrefabs.GetRandomElement(), transform);
        blocks.Add(newBlock);
        newBlock.gameObject.name = $"Block {blocks.Count}";

        // Move it into the correct position
        Vector3 direction = (lastBlock.endPoints[endPoint].position - lastBlock.transform.position).normalized;
        newBlock.transform.position = lastBlock.transform.position + direction * 11;

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


        // Check for dead ends - BUGGED!! Always claims a dead end
        foreach (Transform newBlockEndPoint in newBlock.endPoints)
        {
            //RaycastHit[] hits = Physics.RaycastAll(raycastOrigin + Vector3.up, Vector3.down, 5);
            Collider[] hits = Physics.OverlapSphere(newBlockEndPoint.transform.position, 5);
            Debug.Log($"Hits: {hits.Length}");
            foreach (Collider hit in hits)
            {
                //Debug.Log($"Hit: {hit.gameObject.name}");
                if (hit.TryGetComponent(out Block hitBlock))
                {
                    Debug.Log($"Hit block");
                    if (hitBlock == newBlock)
                    {
                        continue;
                    }
                    Debug.Log($"DeadEnd!");

                    var rot = newBlock.transform.rotation;
                    var pos = newBlock.transform.position;
                    
                    DestroyImmediate(newBlock);
                    newBlock = Instantiate(deadEndBlock, transform);
                    newBlock.transform.rotation = rot;
                    newBlock.transform.position = pos;
                    break;
                }
            }

        }

        lastBlock = newBlock;
        SpawnButtons();
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
            Vector3 direction = (lastBlock.endPoints[i].position - lastBlock.transform.position).normalized;
            var spawnPosition = lastBlock.transform.position + direction * 11;
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