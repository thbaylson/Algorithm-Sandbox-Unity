using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class SpawnEntitiesSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<SpawnEntitiesConfig>();
    }

    protected override void OnUpdate()
    {
        // Spawn Entities when the space key is pressed.
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        SpawnEntitiesConfig config = SystemAPI.GetSingleton<SpawnEntitiesConfig>();
        RotateSpeed rotateSpeedRef = EntityManager.GetComponentData<RotateSpeed>(config.prefabEntity);

        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(WorldUpdateAllocator);
        foreach (var localTransformRef in SystemAPI.Query<RefRO<LocalTransform>>())
        {
            Entity spawnedEntity = entityCommandBuffer.Instantiate(config.prefabEntity);

            // Because prefab entity properties are set at bake time, these runtime entities need to be set manually.
            entityCommandBuffer.SetComponent(spawnedEntity, new LocalTransform
            {
                Position = localTransformRef.ValueRO.Position,
                Scale = 1f,
                Rotation = quaternion.identity,
            });

            entityCommandBuffer.SetComponent(spawnedEntity, new RotateSpeed
            {
                xSpeed = rotateSpeedRef.xSpeed + Random.Range(-5f, 5f),
                ySpeed = rotateSpeedRef.ySpeed + Random.Range(-5f, 5f),
                zSpeed = rotateSpeedRef.zSpeed + Random.Range(-5f, 5f)
            });

            entityCommandBuffer.SetComponent(spawnedEntity, new Movement
            {
                movement = new float3
                {
                    x = Random.Range(-1f, 1f),
                    y = Random.Range(-1f, 1f),
                    z = Random.Range(0.01f, 1f)
                }
            });
        }

        entityCommandBuffer.Playback(EntityManager);
    }
}
