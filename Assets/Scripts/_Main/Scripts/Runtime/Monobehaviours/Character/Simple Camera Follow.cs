using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform target;
    public float damping = 0.2f;

    private Vector3 offset;

    void Start()
    {
        // Calculate the initial offset.
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, damping);
            transform.position = smoothedPosition;
        }
    }
}

