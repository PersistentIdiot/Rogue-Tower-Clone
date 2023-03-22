using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        lastBlock = blocks[0];
        SpawnNewBlock(lastBlock);
        SpawnNewBlock(lastBlock);
    }

    private void SpawnNewBlock(Block inputLastBlock)
    {
        Debug.Log($"{nameof(Map)}.{nameof(SpawnNewBlock)}() ");
        var newBlock = Instantiate(blockPrefabs.GetRandomElement(), transform);
        blocks.Add(newBlock);
        newBlock.gameObject.name = $"Block {blocks.Count}";
        var newBlockEnd = newBlock.GetComponentsInChildren<PathPoint>().FirstOrDefault(point => point.IsEnd);
        var lastBlockStart = inputLastBlock.GetComponentsInChildren<PathPoint>().FirstOrDefault(point => point.IsStart);

        Debug.Assert(newBlockEnd != null);
        Debug.Assert(lastBlockStart != null);

        Vector3 direction = (lastBlockStart.transform.position - inputLastBlock.transform.position).normalized;
        newBlock.transform.position = blocks[0].transform.position + direction * 11;
        
        for (int i = 0; i < 3; i++)
        {
            newBlock.transform.rotation = Quaternion.Euler(0, 0, i * 90);
            if (!Physics.SphereCast(newBlockEnd.transform.position, 1, newBlockEnd.transform.position - lastBlockStart.transform.position, out _, 3))
            {
                continue;
            }

            Debug.Log($"{nameof(Map)}.{nameof(SpawnNewBlock)}() - Connected properly!");
            break;
        }

        lastBlock = newBlock;
    }
}