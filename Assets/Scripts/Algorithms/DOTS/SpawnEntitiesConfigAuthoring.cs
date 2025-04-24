using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct SpawnEntitiesConfig : IComponentData
{
    public Entity prefabEntity;
}

public class SpawnEntitiesConfigAuthoring : MonoBehaviour
{
    public GameObject prefab;

    public class Baker : Baker<SpawnEntitiesConfigAuthoring>
    {
        public override void Bake(SpawnEntitiesConfigAuthoring authoring)
        {
            // The spawner will not move, so the transform is not needed.
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new SpawnEntitiesConfig
            {
                prefabEntity = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
            });
        }
    }
}
