using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // Set the billboard to face the camera
        mainCamera = Camera.main;
        //if (mainCamera != null)
        //{
        //    transform.LookAt(mainCamera.transform);
        //    transform.Rotate(0, 180, 0); // Rotate to face the camera correctly
        //}
    }

    void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
