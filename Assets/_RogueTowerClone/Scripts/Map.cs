using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nomnom.RaycastVisualization;
using Palmmedia.ReportGenerator.Core.Logging;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Block startingBlock;
    [SerializeField] private List<Block> blockPrefabs = new List<Block>();

    private List<Block> blocks = new List<Block>();
    private Block lastBlock;

    private void Start()
    {
        blocks.Add(Instantiate(startingBlock, transform));
        lastBlock = startingBlock;
    }

    public void SpawnNewBlock()
    {
        var newBlock = Instantiate(blockPrefabs.GetRandomElement(), transform);
        blocks.Add(newBlock);
        newBlock.gameObject.name = $"Block {blocks.Count}";
        var newBlockEnd = newBlock.GetComponentsInChildren<PathPoint>().FirstOrDefault(point => point.IsEnd);
        var lastBlockStart = lastBlock.GetComponentsInChildren<PathPoint>().FirstOrDefault(point => point.IsStart);
        var lastBlockEnd = lastBlock.GetComponentsInChildren<PathPoint>().FirstOrDefault(point => point.IsEnd);

        Debug.Assert(newBlockEnd != null);
        Debug.Assert(lastBlockStart != null);
        Debug.Assert(lastBlockEnd != null);

        Vector3 direction = (lastBlockStart.transform.position - lastBlock.transform.position).normalized;
        newBlock.transform.position = lastBlock.transform.position + direction * 11;

        for (int i = 0; i < 3; i++)
        {
            direction = newBlockEnd.transform.position - newBlock.transform.position;
            Debug.DrawRay(newBlockEnd.transform.position, direction, Color.red, 999);
            if (VisualPhysics.SphereCast(newBlockEnd.transform.position, 3, direction * 2, out RaycastHit hit))
            {
                Debug.Log($"Hit: {hit.collider.gameObject.name}");
                break;
            }

            newBlock.transform.rotation = Quaternion.Euler(0, 0, i * 90);
        }


        lastBlock = newBlock;
    }
}