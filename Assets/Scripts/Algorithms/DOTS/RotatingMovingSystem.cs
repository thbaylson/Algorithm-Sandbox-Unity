using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct RotatingMovingSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;

        // If your system Query has too many types, you can use Aspects to group them.
        foreach (var aspect in SystemAPI.Query<RotatingMovingAspect>())
        {
            // Some logic to perform on the aspect.
            aspect.MoveAndRotate(SystemAPI.Time.DeltaTime);
        }
    }
}
