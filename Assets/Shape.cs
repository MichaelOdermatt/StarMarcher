using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public List<Transform> _controlPoints;

    private void Start()
    {
        _controlPoints = OrderPointsByAngle(_controlPoints);
        DrawLineThroughPoints(_controlPoints);
    }

    private void DrawLineThroughPoints(List<Transform> points)
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            return;

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.useWorldSpace = true;
        lineRenderer.loop = true;
        lineRenderer.positionCount = points.Count;

        List<Vector3> positions = new List<Vector3>();
        foreach (var point in points)
        { 
            positions.Add(point.position);
        }

        lineRenderer.SetPositions(positions.ToArray());

    }

    private List<Transform> OrderPointsByAngle(List<Transform> points)
    {
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
