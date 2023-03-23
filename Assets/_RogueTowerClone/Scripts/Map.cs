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
        newBlock.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        var forwardFrom = newBlock.toStart;
        var forwardTo = -lastBlock.toEnd;
        var rotation = Quaternion.FromToRotation(forwardFrom, forwardTo);
        newBlock.transform.rotation *= rotation;

        // Orients in case it goes upside down
        float dot = Vector3.Dot(newBlock.transform.up, Vector3.up);
        if (dot <= 0)
        {
            newBlock.transform.rotation *= Quaternion.AngleAxis(180, Vector3.forward);
        }

        lastBlock = newBlock;
        SpawnButtons();
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
        Vector3 direction = (lastBlock.end.position - lastBlock.transform.position).normalized;
        var buttonPosition = lastBlock.transform.position + direction * 11+ new Vector3(0,5,0);

        var newButton = Instantiate(buttonPrefab, canvas);
        spawnTileButtons.Add(newButton);
        newButton.transform.position = buttonPosition;
        newButton.onClick.AddListener(SpawnNewBlock);
    }
}