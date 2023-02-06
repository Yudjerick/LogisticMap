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
        pathCreator.StartLine();
        _isDrawing = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var point = pathCreator.CalculateLocalPoint(eventData.pointerCurrentRaycast.worldPosition);
        pathCreator.points.Add(point);
        _isDrawing = false;
        pathCreator.BakeLine();

    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (_isDrawing)
        {
            var point = pathCreator.CalculateLocalPoint(eventData.pointerCurrentRaycast.worldPosition);
            pathCreator.points.Add(point);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isDrawing = false;
    }
}
