using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    [SerializeField] private float sensitivity = 1;
    Vector3 lastMousePosition;

    private void Update()
    {
        MouseInputs();
    }

    void MouseInputs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }
        else if(Input.GetMouseButton(0))
        {
            Vector3 delta = (Input.mousePosition - lastMousePosition)*sensitivity;
 
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
