using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script to make camera follow the player object

public class CameraFollow : MonoBehaviour
{
    // player target for camera
    public Transform target;
    public Vector3 offset;

    private void FixedUpdate()
    {
        transform.position = target.position + offset;
    }
}
