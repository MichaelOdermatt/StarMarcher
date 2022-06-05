using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public float maxY = 1;
    public float maxX = 2;

    public Transform Target;

    private void Update()
    {
        float newXPos = transform.position.x;
        float newYPos = transform.position.y;

        if (transform.position.x > Target.position.x + maxX)
            newXPos = Target.position.x + maxX;

        if (transform.position.x < Target.position.x - maxX)
            newXPos = Target.position.x - maxX;

        if (transform.position.y > Target.position.y + maxY)
            newYPos = Target.position.y + maxY;

        if (transform.position.y < Target.position.y - maxY)
            newYPos = Target.position.y - maxY;

        Vector3 newPos = new Vector3(newXPos, newYPos, -10);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
}
