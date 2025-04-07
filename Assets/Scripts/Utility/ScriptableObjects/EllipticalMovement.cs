using UnityEngine;

[CreateAssetMenu(fileName = "EllipticalMovement", menuName = "Movement Functions/Elliptical Movement")]
public class EllipticalMovement : MovementFunction
{
    // Multiplier for the vertical axis.
    public float verticalMultiplier = 0.5f;

    public override float GetPeriod(float radius)
    {
        // You can define the period similarly to a circle.
        return 2 * Mathf.PI;
    }

    public override Vector2 ComputePosition(float radius, float time)
    {
        // Compute an ellipse: horizontal radius is unchanged, vertical is scaled.
        return new Vector2(radius * Mathf.Cos(time), radius * verticalMultiplier * Mathf.Sin(time));
    }
}
