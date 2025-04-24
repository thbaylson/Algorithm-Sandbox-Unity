using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct MovementSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        // It's considered a good practice to test with SystemAPI.Query before creating a job.
        foreach(var (localTransformRef, movementRef) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Movement>>().WithNone<Player>())
        {
            // TODO: What if we used Unity's physics system instead of updating the position?
            localTransformRef.ValueRW = localTransformRef.ValueRO.Translate(movementRef.ValueRO.movement * SystemAPI.Time.DeltaTime);
        }
    }
}
