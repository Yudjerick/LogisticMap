using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    public GameObject testPoint;
    
    private LineRenderer _lineRenderer;
    public List<Vector2> points;
    [SerializeField] private int lineLayer;
    [SerializeField] private float lineWidthMultiplier;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private GameObject truck;
    [SerializeField] private Material bakedMaterial;
    private Vector3 _lowLeftCorner;
    private float _multiplierX;
    private float _multiplierY;
    private GameObject _line;
    

    private void Start()
    {
        _line = null;
    }

    public void StartLine()
    {
        _line = new GameObject("line")
        {
            layer = lineLayer,
            transform =
            {
                parent = gameObject.transform,
                localPosition = Vector3.zero,
                localRotation = Quaternion.Euler(Vector3.zero)
            }
        };

        _lineRenderer = _line.AddComponent<LineRenderer>();
        _lineRenderer.widthMultiplier = lineWidthMultiplier;
        _lineRenderer.material = lineMaterial;
        _lineRenderer.textureMode = LineTextureMode.Tile;
        lineMaterial.mainTextureScale = new Vector2(1/lineWidthMultiplier,1);
        _lineRenderer.useWorldSpace = false;
        points = new List<Vector2>();
    }

    public void BakeLine()
    {
        if (points.Count < 3)
        {
            Destroy(_line);
            return;
        }

        if (bakedMaterial != null)
        {
            _lineRenderer.material = bakedMaterial;
            _lineRenderer.material.mainTextureScale = new Vector2(1/lineWidthMultiplier,1);
        }

        var instantiate = Instantiate(truck,transform);
        var vehicleMover = instantiate.GetComponent<VehicleMover>();
        vehicleMover.SetPath(points);
        vehicleMover.GoToStartPosition();
        _line = null;
    }
    
    void Update()
    {
        
        _multiplierX = transform.localScale.x / 2;
        _multiplierY = transform.localScale.y / 2;
        _lowLeftCorner = transform.position - (transform.right * _multiplierX + transform.up * _multiplierY);
        if(_line == null)
            return;
        Vector3[] lineRendererPositions = new Vector3[points.Count];
        for (int i = 0; i < points.Count; i++)
        {
            lineRendererPositions[i] = CalculateLineRendererPosition(points[i]);
            //lineRendererPositions[i] = CalculateGlobalPoint(points[i]);
        }

        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(lineRendererPositions);
    }

    Vector3 CalculateLineRendererPosition(Vector2 point)
    {
        return new Vector2((point.x - 1) * _multiplierX, (point.y - 1) * _multiplierY);
    }

    Vector3 CalculateGlobalPoint(Vector2 point)
    {
        Vector3 globalPoint = _lowLeftCorner + (transform.right * (_multiplierX * point.x)) +
                              (transform.up * (_multiplierY * point.y));
        return globalPoint;
    }
    
    public Vector2 CalculateLocalPoint(Vector3 point)
    {
        Vector3 r = point - _lowLeftCorner;
        Vector2 localPoint = new Vector2(Vector3.Dot(r, transform.right)/_multiplierX, Vector3.Dot(r, transform.up)/_multiplierY);
        return localPoint;
    }
}
