using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Unity.Entities;
using Unity.Mathematics;
using System;

public struct Movement : IComponentData
{
    public float3 movement;
}

public class MovementAuthoring : MonoBehaviour
{
    public class Baker : Baker<MovementAuthoring>
    {
        // Note: Bake only happens once for all existing prefabs. Newly instantiated prefabs at runtime will not run this code.
        public override void Bake(MovementAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Movement
            {
                movement = new float3
                {
                    x = Random.Range(-1f, 1f),
                    y = Random.Range(-1f, 1f),
                    z = Random.Range(-1f, 1f)
                }
            });
        }
    }
}
