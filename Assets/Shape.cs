using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Shape : MonoBehaviour
{
    public List<Transform> controlPoints;
    private LineRenderer _lineRenderer;
    private Vector3[] _linePositions;

    public float lineWidth;

    private void Start()
    {
        controlPoints = OrderPointsByAngle(controlPoints);
        if (controlPoints == null)
            return;

        InitializeLineRenderer();
        _linePositions = new Vector3[controlPoints.Count];
    }

    private void Update()
    {
        if (controlPoints == null)
            return;

        UpdateLinePositions(); 
    }

    private void InitializeLineRenderer()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.startWidth = lineWidth;
        _lineRenderer.endWidth = lineWidth;
        _lineRenderer.useWorldSpace = true;
        _lineRenderer.loop = true;
        if (controlPoints.Count > 0)
            _lineRenderer.positionCount = controlPoints.Count;
    }

    private void UpdateLinePositions()
    {
        if (controlPoints.Count == 0)
            return;

        for (int i = 0; i < controlPoints.Count; i++)
        {
            _linePositions[i] = controlPoints[i].position;
        }

        _lineRenderer.SetPositions(_linePositions);
    }

    private static List<Transform> OrderPointsByAngle(List<Transform> points)
    {
        if (points.Count == 0)
            return null;

        Transform rightMost = points[0];
        for (int i = 1; i < points.Count - 1; i++)
        {
            if (points[i].position.x > rightMost.position.x)
                rightMost = points[i];
        }
        points.Remove(rightMost);

        List<Transform> orderedPoints = points.OrderBy(value => Angle360(rightMost.position, value.position)).ToList();

        orderedPoints.Insert(0, rightMost);

        return orderedPoints;
    }

    // https://answers.unity.com/questions/1164731/need-help-getting-angles-to-work-in-360-degrees.html
    public static float Angle360(Vector2 from, Vector2 to)
    {
        float angle = Vector2.SignedAngle(from, to);
        return angle < 0 ? 360 - angle * -1 : angle;
    }
}
