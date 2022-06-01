using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public GameObject player;

    public List<Shape> shapes;
    private List<Transform> nodes;

    private List<Transform> betweenNodes;
    private float distanceAlongEdge = 0f;
    private float movementSpeed = 0.05f;

    private void Start()
    {
        nodes = new List<Transform>();
        foreach (var shape in shapes)
        {
            nodes.AddRange(shape.controlPoints);
        }

        betweenNodes = new List<Transform>();
        betweenNodes.Add(nodes[0]);
        betweenNodes.Add(nodes[1]);
    }

    private void Update()
    {
        distanceAlongEdge += Time.deltaTime * movementSpeed;
        if (betweenNodes.Count > 0)
            player.transform.position = Vector3.Lerp(betweenNodes[0].position, betweenNodes[1].position, distanceAlongEdge);
    }
}
