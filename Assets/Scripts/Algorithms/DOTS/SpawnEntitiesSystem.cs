using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class SpawnEntitiesSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<Player>();
    }

    protected override void OnUpdate()
    {
        // Spawn Entities when the space key is pressed.
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        SpawnEntitiesConfig config = SystemAPI.GetSingleton<SpawnEntitiesConfig>();

        foreach (var localTransformRef in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Player>())
        {
            Entity spawnedEntity = EntityManager.Instantiate(config.prefabEntity);
            EntityManager.SetComponentData(spawnedEntity, new LocalTransform
            {
                Position = localTransformRef.ValueRO.Position,
                Scale = 1f,
                Rotation = quaternion.identity,
            });
        }

        // This is one option for instaniating
        // This makes it so that the below code only runs once.
        //Enabled = false;
        //SpawnEntitiesConfig config = SystemAPI.GetSingleton<SpawnEntitiesConfig>();
        // EntityManager.Instantiate can also spawn multiple entities all at once, which could be more efficient.
        //for (int i = 0; i < config.amount; i++)
        //{
        //    Entity spawnedEntity = EntityManager.Instantiate(config.prefabEntity);
        //    // EntityManager.SetComponentData works in the same way, but may be less performant.
        //    SystemAPI.SetComponent(spawnedEntity, new LocalTransform
        //    {
        //        Position = new float3
        //        {
        //            x = Random.Range(-5f, 5f),
        //            y = Random.Range(-5f, 5f),
        //            z = Random.Range(-5f, 5f)
        //        },
        //        Scale = 1f,
        //        Rotation = quaternion.identity,
        //    });
        //}
    }
}
