using System;
using UnityEngine;

public class FunctionalMover : MonoBehaviour
{
    public float Period { get; private set; }

    [SerializeField] private Func<float, float, (float, float)> _function;
    [SerializeField] private bool _isMoving = false;
    
    private float _radius = 1f;
    private float _age = 0f;

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            // This will reset the age timer every period to prevent data type overflow
            _age = (_age + Time.deltaTime) % Period;

            // TODO: It would be better to make this velocity based instead of position based
            (float, float) newPos = _function.Invoke(_radius, _age);
            transform.localPosition = new Vector3(newPos.Item1, newPos.Item2);
        }
    }

    // Set Function
    public void SetFunction(Func<float, float, (float, float)> function)
    {
        _function = function;

        if(_function == FunctionOfTime.Circle)
        {
            // The period of an object is the length of time required for one complete rotation of a circle. It is equal to
            // circumference divided by speed.
            Period = 2 * (float)Math.PI * _radius;
        }
    }

    // Start/Stop Moving
    public void SetIsMoving(bool isMoving)
    {
        _isMoving = isMoving;
    }

    // Place the object at its initial position. This prevents objects spawning at the origin and then jumping to their position
    public void PlaceAtInitialPosition()
    {
        (float, float) newPos = _function.Invoke(_radius, 0f);
        transform.localPosition = new Vector3(newPos.Item1, newPos.Item2);
    }
}

public class FunctionOfTime
{
    public static (float, float) Circle(float radius, float time)
    {
        return (radius * (float)Math.Cos(time), radius * (float)Math.Sin(time));
    }
}