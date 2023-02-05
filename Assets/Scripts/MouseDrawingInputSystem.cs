using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDrawingInputSystem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler, IPointerExitHandler
{
    [SerializeField] private PathRenderer pathRenderer;
    private bool _isDrawing;

    public void OnPointerDown(PointerEventData eventData)
    {
        pathRenderer.StartLine();
        _isDrawing = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var point = pathRenderer.CalculateLocalPoint(eventData.pointerCurrentRaycast.worldPosition);
        pathRenderer.points.Add(point);
        _isDrawing = false;
        pathRenderer.BakeLine();

    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (_isDrawing)
        {
            var point = pathRenderer.CalculateLocalPoint(eventData.pointerCurrentRaycast.worldPosition);
            pathRenderer.points.Add(point);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isDrawing = false;
    }
}
