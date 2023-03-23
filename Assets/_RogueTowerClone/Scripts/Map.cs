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
        // Instantiate new block, add to list, name it
        var newBlock = Instantiate(blockPrefabs.GetRandomElement(), transform);
        blocks.Add(newBlock);
        newBlock.gameObject.name = $"Block {blocks.Count}";

        // Move it into the correct position
        Vector3 direction = (lastBlock.end.position - lastBlock.transform.position).normalized;
        newBlock.transform.position = lastBlock.transform.position + direction * 11;


        // Orient new block ( via Nom - https://discord.com/channels/750329891383410728/983851080255418408/1088325758050648134 )
        newBlock.root.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        var forwardFrom = newBlock.toStart;
        var forwardTo = -lastBlock.toEnd;
        var rotation = Quaternion.FromToRotation(forwardFrom, forwardTo);
        newBlock.root.rotation *= rotation;

        // Orients in case it goes upside down
        float dot = Vector3.Dot(newBlock.root.up, Vector3.up);
        if (dot <= 0)
        {
            newBlock.root.rotation *= Quaternion.AngleAxis(180, Vector3.forward);
        }

        lastBlock = newBlock;
    }
}