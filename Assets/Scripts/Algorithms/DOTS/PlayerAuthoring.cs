using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct Player : IComponentData
{
    // No data needed. This is just to tag the player object.
}

public class PlayerAuthoring : MonoBehaviour
{
    public class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Player());
        }
    }
}
