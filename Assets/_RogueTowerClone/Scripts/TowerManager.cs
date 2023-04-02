using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private GameObject cursorPrefab;
    [SerializeField] private Tower towerPrefab;
    [SerializeField] private float heightOffset;
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
            LayerMask mask = LayerMask.GetMask("Grass");
            var hits = Physics.RaycastAll(rayOrigin, Single.PositiveInfinity, mask);

            bool validHit = false;
            Vector3 hitPoint = Vector3.zero;

            foreach (RaycastHit hit in hits)
            {
                if (Vector3.Dot(Vector3.up, hit.normal) > 0.95f)
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

                if (Input.GetMouseButtonDown(0))
                {
                    var tower = Instantiate(towerPrefab);
                    tower.transform.position = cursorPosition;
                }
            }
        }
        else
        {
            cursor.gameObject.SetActive(false);
        }
    }
}