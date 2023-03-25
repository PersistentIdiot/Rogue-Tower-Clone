using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private GameObject cursorPrefab;
    [SerializeField] private Vector3 cursorOffset = Vector3.up;
    private GameObject cursor;
    private bool placingTower = false;

    private void Awake()
    {
        cursor = Instantiate(cursorPrefab);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            placingTower = !placingTower;
        }

        if (placingTower)
        {
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hits = Physics.RaycastAll(rayOrigin);
            bool validHit = false;
            Vector3 hitPoint = Vector3.zero;
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.TryGetComponent(out Tile tile) && tile.TileType == TileTypes.Grass)
                {
                    validHit = true;
                    hitPoint = hit.point;
                }
            }

            cursor.gameObject.SetActive(validHit);
            if (validHit)
            {
                var cursorPosition = hitPoint;
                cursorPosition.x = (float)Math.Round(hitPoint.x);
                cursorPosition.z = (float)Math.Round(hitPoint.z);
                cursor.transform.position = cursorPosition;
            }
        }
        else
        {
            cursor.gameObject.SetActive(false);
        }
    }
}