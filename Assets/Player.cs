using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float RotationSpeed = 4f;
    private float Radius = 1f;

    public Transform nodeTransform;
    private float angle = 0;

    private void Update()
    {
        if (nodeTransform != null)
            RotateAroundNode();
    }

    private void RotateAroundNode()
    {
        angle += RotationSpeed * Time.deltaTime;

        var offset = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle)) * Radius;
        transform.position = nodeTransform.position + offset;
    }
}
