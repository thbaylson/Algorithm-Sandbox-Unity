using UnityEngine;

[CreateAssetMenu(fileName = "CircleMovement", menuName = "Movement Functions/Circle Movement")]
public class CircleMovement : MovementFunction
{
    public override float GetPeriod(float radius)
    {
        return 2 * Mathf.PI * radius;
    }

    public override Vector2 ComputePosition(float radius, float time)
    {
        return new Vector2(radius * Mathf.Cos(time), radius * Mathf.Sin(time));
    }
}
