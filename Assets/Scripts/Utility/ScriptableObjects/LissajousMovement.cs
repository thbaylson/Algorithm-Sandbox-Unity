using UnityEngine;

[CreateAssetMenu(fileName = "LissajousMovement", menuName = "Movement Functions/Lissajous Movement")]
public class LissajousMovement : MovementFunction
{
    // Frequency multipliers for each axis.
    public float a = 3f;  // X-axis frequency
    public float b = 2f;  // Y-axis frequency
    // Phase difference between the two sine waves.
    public float delta = Mathf.PI / 2;

    public override float GetPeriod(float radius)
    {
        // A simple period; for more complex behavior, you could compute a least common period based on a and b.
        return 2 * Mathf.PI;
    }

    public override Vector2 ComputePosition(float radius, float time)
    {
        float x = radius * Mathf.Sin(a * time + delta);
        float y = radius * Mathf.Sin(b * time);
        return new Vector2(x, y);
    }
}
