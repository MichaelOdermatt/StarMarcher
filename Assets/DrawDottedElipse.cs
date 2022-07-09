using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDottedElipse : MonoBehaviour
{
    public LineRenderer circleRenderer;

    private void Start()
    {
        DrawCircle(100, 1);
        float width = circleRenderer.startWidth;
        circleRenderer.material.mainTextureScale = new Vector2(1f / width, 1.0f);
    }

    private void DrawCircle(int steps, float radius)
    {
        circleRenderer.positionCount = steps + 1;

        for (int i = 0; i <= steps; i++)
        {
            float circumferenceProgress = (float)i / steps;

            float radian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(radian);
            float yScaled = Mathf.Sin(radian);

            float x = gameObject.transform.position.x + (xScaled * radius);
            float y = gameObject.transform.position.y + (yScaled * radius);

            Vector3 currentPos = new Vector3((float)x, (float)y, 0);

            circleRenderer.SetPosition(i, currentPos);
        }
    }
}
