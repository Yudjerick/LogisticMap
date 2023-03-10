using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDrawingInputSystem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler, IPointerExitHandler
{
    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private PathDestroyer pathDestroyer;
    private bool _isDrawing;
    [SerializeField] private bool erase;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (erase)
        {
            var point = pathCreator.CalculateLocalPoint(eventData.pointerCurrentRaycast.worldPosition);
            pathDestroyer.DestroyLine(point);
            return;
        }
        pathCreator.CreateLine();
        _isDrawing = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (erase)
        {
            var point = pathCreator.CalculateLocalPoint(eventData.pointerCurrentRaycast.worldPosition);
            pathDestroyer.DestroyLine(point);
            return;
        }
        if (_isDrawing)
        {
            FinishDrawing(eventData);
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (erase)
        {
            var point = pathCreator.CalculateLocalPoint(eventData.pointerCurrentRaycast.worldPosition);
            pathDestroyer.DestroyLine(point);
            return;
        }
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
