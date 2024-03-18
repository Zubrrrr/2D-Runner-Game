using System.Collections.Generic;
using UnityEngine;

public class SmoothFlightWithCurve : MonoBehaviour
{
    [Tooltip("List of points through which the object will move")]
    [SerializeField] private List<Vector3> _points;

    [Tooltip("Total duration of the movement from one point to another")]
    [SerializeField] private float _duration = 5f;

    [Tooltip("Curve to smooth the movement between points")]
    [SerializeField] private AnimationCurve _flightCurve;

    private int _currentPointIndex = 0;
    private float _elapsedTime = 0f;
    private bool _hasReachedFinalPoint = false;

    void FixedUpdate()
    {
        MoveThroughPoints();
    }

    private void MoveThroughPoints()
    {
        if (_points.Count < 2 || _hasReachedFinalPoint) return;

        float fraction = CalculateFractionOfJourney();

        Vector3 currentStartPoint = _points[_currentPointIndex];
        Vector3 currentEndPoint = _points[(_currentPointIndex + 1) % _points.Count];

        transform.position = InterpolatePosition(currentStartPoint, currentEndPoint, fraction);

        UpdateTiming();

        if (fraction >= 1f)
        {
            PrepareForNextPoint();
        }
    }

    private float CalculateFractionOfJourney()
    {
        return _elapsedTime / _duration;
    }

    private Vector3 InterpolatePosition(Vector3 start, Vector3 end, float fraction)
    {
        float curveValue = _flightCurve.Evaluate(fraction);
        return Vector3.Lerp(start, end, curveValue);
    }

    private void UpdateTiming()
    {
        _elapsedTime += Time.deltaTime;
    }

    private void PrepareForNextPoint()
    {
        _elapsedTime = 0f;
        _currentPointIndex++;

        if (_currentPointIndex >= _points.Count - 1)
        {
            _hasReachedFinalPoint = true;
        }
    }
}
