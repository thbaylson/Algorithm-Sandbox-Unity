using UnityEngine;

[CreateAssetMenu(fileName = "SpiralMovement", menuName = "Movement Functions/Spiral Movement")]
public class SpiralMovement : MovementFunction
{
    // Controls how much the radius increases per unit time.
    public float growthRate = 0.5f;

    public override float GetPeriod(float radius)
    {
        // For one full rotation, the period can remain similar to a circle.
        return 2 * Mathf.PI;
    }

    public override Vector2 ComputePosition(float radius, float time)
    {
        // The angle rotates like in a circle.
        float angle = time;
        // Increase the radius based on the growth rate.
        float currentRadius = radius + growthRate * time;
        return new Vector2(currentRadius * Mathf.Cos(angle), currentRadius * Mathf.Sin(angle));
    }
}
