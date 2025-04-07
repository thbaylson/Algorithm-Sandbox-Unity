using UnityEngine;

public abstract class MovementFunction : ScriptableObject
{
    // Returns the period based on the provided radius.
    public abstract float GetPeriod(float radius);

    // Computes the new position given a radius and elapsed time.
    public abstract Vector2 ComputePosition(float radius, float time);
}
