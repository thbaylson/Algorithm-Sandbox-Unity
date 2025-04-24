using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

// Components must be structs. They should not contain any functions.
// All of the properties in a component should be public.
// This struct does not necessarily need to be in the same script, but it looks more organized this way.
public struct RotateSpeed : IComponentData
{
    public float xSpeed;
    public float ySpeed;
    public float zSpeed;
}

// Appending "Authoring" to the name is just a convention. But the class name MUST be the same as the file name.
public class RotateSpeedAuthoring : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    public float zSpeed;

    // This placement and naming is also a convention.
    private class Baker : Baker<RotateSpeedAuthoring>
    {
        public override void Bake(RotateSpeedAuthoring authoring)
        {
            // We need the Dynamic flag because we intend to move/rotate this object.
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new RotateSpeed
            {
                xSpeed = authoring.xSpeed + Random.Range(-5f, 5f),
                ySpeed = authoring.ySpeed + Random.Range(-5f, 5f),
                zSpeed = authoring.zSpeed + Random.Range(-5f, 5f)
            });
        }
    }
}
