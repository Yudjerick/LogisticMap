using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDrawingInputSystem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler, IPointerExitHandler
{
    [SerializeField] private PathCreator pathCreator;
    private bool _isDrawing;

    public void OnPointerDown(PointerEventData eventData)
    {
        pathCreator.CreateLine();
        _isDrawing = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isDrawing)
        {
            FinishDrawing(eventData);
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (_isDrawing)
        {
            var point = pathCreator.CalculateLocalPoint(eventData.pointerCurrentRaycast.worldPosition);
            if (point.x != -0.0625f && point.y != 0f)
            {
                pathCreator.points.Add(point);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(_isDrawing)
            FinishDrawing(eventData);
    }

    void FinishDrawing(PointerEventData eventData)
    {
        var point = pathCreator.CalculateLocalPoint(eventData.pointerCurrentRaycast.worldPosition);
        if (point.x != -0.0625f && point.y != 0f)
        {
            pathCreator.points.Add(point);
        }
        _isDrawing = false;
        pathCreator.BakeLine();
    }
}
