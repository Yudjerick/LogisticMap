using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
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

    public void CreateLine()
    {
        _line = new GameObject("line")
        {
            layer = lineLayer,
            transform =
            {
                parent = gameObject.transform,
                localPosition = Vector3.zero,
                localRotation = Quaternion.Euler(Vector3.zero),
                localScale = Vector3.one
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

        var instantiate = Instantiate(truck,_line.transform);
        var vehicleMover = instantiate.GetComponent<VehicleMover>();
        vehicleMover.SetPath(points);
        vehicleMover.GoToStartPosition();
        _line = null;
    }
    
    void Update()
    {
        _multiplierX = transform.localScale.x;
        _multiplierY = transform.localScale.y;
        _lowLeftCorner = transform.position - (transform.right * _multiplierX + transform.up * _multiplierY);
        if(_line == null)
            return;
        Vector3[] lineRendererPositions = new Vector3[points.Count];
        for (int i = 0; i < points.Count; i++)
        {
            lineRendererPositions[i] = points[i];
        }

        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(lineRendererPositions);
    }
    public Vector2 CalculateLocalPoint(Vector3 point)
    {
        Vector3 r = point - transform.position;
        Vector2 localPoint = new Vector2(Vector3.Dot(r, transform.right)/_multiplierX, Vector3.Dot(r, transform.up)/_multiplierY);
        return localPoint;
    }
}
