using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct RotatingMovingAspect : IAspect
{
    // It looks strange, but it is perfectly valid to mix the readonly keyword with RefRW.
    public readonly RefRW<LocalTransform> localTransformRef;
    public readonly RefRO<RotateSpeed> rotateSpeedRef;
    public readonly RefRO<Movement> movementRef;

    public void MoveAndRotate(float deltaTime)
    {
        localTransformRef.ValueRW = localTransformRef.ValueRO.Translate(movementRef.ValueRO.movement * deltaTime);

        localTransformRef.ValueRW = localTransformRef.ValueRO.RotateX(rotateSpeedRef.ValueRO.xSpeed * deltaTime);
        localTransformRef.ValueRW = localTransformRef.ValueRO.RotateY(rotateSpeedRef.ValueRO.ySpeed * deltaTime);
        localTransformRef.ValueRW = localTransformRef.ValueRO.RotateZ(rotateSpeedRef.ValueRO.zSpeed * deltaTime);
    }
}
