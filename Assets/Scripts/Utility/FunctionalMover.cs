using UnityEngine;

public class FunctionalMover : MonoBehaviour
{
    public float Period { get; private set; }

    // Reference to a ScriptableObject implementing the movement logic.
    [SerializeField] private MovementFunction movementFunction;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _radius = 2f;
    [SerializeField] private bool _isMoving = false;

    private float _age = 0f;

    private void FixedUpdate()
    {
        if (_isMoving && movementFunction != null)
        {
            // Update the age and wrap it around the period.
            // TODO: Move age logic out of this completely. ObjectPool should handle age. This will also fix SpiralMovement.
            _age = (_age + Time.deltaTime * _moveSpeed) % Period;

            Vector2 newPos = movementFunction.ComputePosition(_radius, _age);
            transform.localPosition = new Vector3(newPos.x, newPos.y, transform.localPosition.z);
        }
    }

    // Set Function
    public void SetMovementFunction(MovementFunction function)
    {
        movementFunction = function;
        if (movementFunction != null)
            Period = movementFunction.GetPeriod(_radius);
    }

    public void SetMoveSpeed(float speed)
    {
        _moveSpeed = speed;
    }

    public void SetRadius(float radius)
    {
        _radius = radius;
    }

    // Start/Stop Moving
    public void SetIsMoving(bool isMoving)
    {
        _isMoving = isMoving;
    }

    // Place the object at its initial position. This prevents objects spawning at the origin and then jumping to their position
    public void PlaceAtInitialPosition()
    {
        if (movementFunction != null)
        {
            Vector2 newPos = movementFunction.ComputePosition(_radius, 0f);
            transform.localPosition = new Vector3(newPos.x, newPos.y, transform.localPosition.z);
        }
    }
}
