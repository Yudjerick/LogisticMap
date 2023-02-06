using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMover : MonoBehaviour
{
    public Vector2[] points;
    [SerializeField] private float speed;
    [SerializeField] private float maxDistance;
    private int _targetIndex;
    private int _direction;

    void Start()
    {
        _targetIndex = 0;
        _direction = 1;
    }
    void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition,points[_targetIndex], Time.deltaTime * speed);
        if (Mathf.Abs(((Vector2)transform.localPosition - points[_targetIndex]).magnitude) <= maxDistance)
        {
            _targetIndex+=_direction;
            if (_targetIndex == points.Length && _direction == 1)
            {
                _targetIndex = points.Length - 2;
                _direction = -1;
            }
            else if(_targetIndex == 0 && _direction == -1)
            {
                _targetIndex = 1;
                _direction = 1;
            }
        }
    }

    public void SetPath(List<Vector2> path)
    {
        points = path.ToArray();
    }

    public void GoToStartPosition()
    {
        transform.localPosition = points[0];
    }
    
}
