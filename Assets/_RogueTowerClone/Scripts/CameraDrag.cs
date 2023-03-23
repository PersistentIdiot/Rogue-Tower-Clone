using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    [SerializeField] private float moveSensitivity = 0.1f;
    [SerializeField] private float zoomSensitivity = 1;
    [SerializeField] private float yMin = 5;
    [SerializeField] private float yMax = 200;
    Vector3 lastMousePosition;

    private void Update()
    {
        MovementInputs();
        ScrollInputs();
    }

    private void ScrollInputs()
    {
        float delta = Input.mouseScrollDelta.y;
        Vector3 newPosition = transform.position;
        newPosition.y = Mathf.Clamp(newPosition.y - delta * zoomSensitivity, yMin, yMax);

        transform.position = newPosition;
    }

    private void MovementInputs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 delta = (Input.mousePosition - lastMousePosition) * moveSensitivity;

            MoveCamera(delta.x, delta.y);

            lastMousePosition = Input.mousePosition;
        }
    }

    private void MoveCamera(float xInput, float zInput)
    {
        float zMove = Mathf.Cos(transform.eulerAngles.y * Mathf.PI / 180) * zInput - Mathf.Sin(transform.eulerAngles.y * Mathf.PI / 180) * xInput;
        float xMove = Mathf.Sin(transform.eulerAngles.y * Mathf.PI / 180) * zInput + Mathf.Cos(transform.eulerAngles.y * Mathf.PI / 180) * xInput;

        transform.position = transform.position + new Vector3(-xMove, 0, -zMove);
    }
}