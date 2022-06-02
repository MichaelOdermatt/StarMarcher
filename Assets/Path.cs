using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public GameObject player;

    public List<Shape> shapes;
    private List<Transform> nodes;

    //private List<Transform> betweenNodes;
    private int nextNode = 1;
    private int prevNode = 0;
    private float distanceAlongEdge = 0f;
    private float movementSpeed = 0.05f;

    private void Start()
    {
        nodes = new List<Transform>();
        foreach (var shape in shapes)
        {
            nodes.AddRange(shape.controlPoints);
        }
    }

    private void Update()
    {
        distanceAlongEdge += Time.deltaTime * movementSpeed;
        if (player.transform.position != nodes[nextNode].position)
            player.transform.position = Vector3.Lerp(nodes[prevNode].position, nodes[nextNode].position, distanceAlongEdge);
    }
}
