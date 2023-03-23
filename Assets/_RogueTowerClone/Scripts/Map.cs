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

        Debug.Assert(newBlockEnd != null);
        Debug.Assert(lastBlockStart != null);

        Vector3 direction = (lastBlockStart.transform.position - lastBlock.transform.position).normalized;
        newBlock.transform.position = lastBlock.transform.position + direction * 11;
        newBlock.root = newBlock.transform;

        var _from = newBlock;
        var _to = lastBlock;

        var forwardFrom = _from.toStart;
        var forwardTo = -_to.toEnd;

        var rotation = Quaternion.FromToRotation(forwardFrom, forwardTo);
        _from.root.transform.rotation = rotation * _from.root.rotation;

        lastBlock = newBlock;
    }
}