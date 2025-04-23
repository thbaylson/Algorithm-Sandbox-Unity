using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;

// Two options for making a system: a class that extends SystemBase (used for managed types) or a struct that extends ISystem
public partial struct RotatingSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        // This ensures the system is only run when the RotateSpeed component is present in the scene.
        state.RequireForUpdate<RotateSpeed>();
    }

    [BurstCompile]// This only works on unmanaged types.
    public void OnUpdate(ref SystemState state)
    {
        // RefRO tells the system we want to read only. Use RefRW for read and write.
        // Multiple jobs could read at the same time, but only one job can write at a time.
        //foreach (var (localTransformRef, rotateSpeedRef) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateSpeed>>())
        //{
        //    localTransformRef.ValueRW = localTransformRef.ValueRO.RotateX(rotateSpeedRef.ValueRO.value * SystemAPI.Time.DeltaTime);
        //    localTransformRef.ValueRW = localTransformRef.ValueRO.RotateY(rotateSpeedRef.ValueRO.value * SystemAPI.Time.DeltaTime);
        //    localTransformRef.ValueRW = localTransformRef.ValueRO.RotateZ(rotateSpeedRef.ValueRO.value * SystemAPI.Time.DeltaTime);
        //}
        // NOTE: The above is commented out because the code below is more efficient.

        RotatingJob rotatingJob = new RotatingJob { deltaTime = SystemAPI.Time.DeltaTime };
        // For IJobEntity this syntax could be considered a longer form than necessary, but other IJob types require it.
        //state.Dependency = rotatingJob.Schedule(state.Dependency);
        rotatingJob.ScheduleParallel();
    }

    [BurstCompile]
    public partial struct RotatingJob : IJobEntity
    {
        public float deltaTime;

        public void Execute(ref LocalTransform localTransformRef, in RotateSpeed rotateSpeed)
        {
            localTransformRef = localTransformRef.RotateX(rotateSpeed.value * deltaTime);
            localTransformRef = localTransformRef.RotateY(rotateSpeed.value * deltaTime);
            localTransformRef = localTransformRef.RotateZ(rotateSpeed.value * deltaTime);
        }
    }
}
