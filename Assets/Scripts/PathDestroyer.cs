using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PathDestroyer : MonoBehaviour
{
    [SerializeField] private float eraseRadius;
    public void DestroyLine(Vector2 eraserPosition)
    {
        LineRenderer[] lineRenderers = GetLineRenderers();
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            for (int j = 0; j < lineRenderers[i].positionCount; j++)
            {
                Vector2 pointPosition = lineRenderers[i].GetPosition(j);
                if ((pointPosition - eraserPosition).magnitude <= eraseRadius)
                {
                    Destroy(lineRenderers[i].gameObject);
                }
            }
        }
    }

    LineRenderer[] GetLineRenderers()
    {
        List<LineRenderer> result = new List<LineRenderer>();
        for (int i = 0; i < transform.childCount; i++)
        {
            LineRenderer lineRenderer = transform.GetChild(i).GetComponent<LineRenderer>();
            if (lineRenderer)
            {
                result.Add(lineRenderer);
            }
        }

        return result.ToArray();
    }
}
