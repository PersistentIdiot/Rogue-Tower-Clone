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

        Vector3 direction = (lastBlock.end.position - lastBlock.transform.position).normalized;
        newBlock.transform.position = lastBlock.transform.position + direction * 11;

        
        newBlock.root.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

        var forwardFrom = newBlock.toStart;
        var forwardTo = -lastBlock.toEnd;

        var rotation = Quaternion.FromToRotation(forwardFrom, forwardTo);
        newBlock.root.rotation *= rotation;

        // orients in case it goes upside down
        float dot = Vector3.Dot(newBlock.root.up, Vector3.up);
        if (dot <= 0)
        {
            newBlock.root.rotation *= Quaternion.AngleAxis(180, Vector3.forward);
        }
        
        lastBlock = newBlock;
    }
}