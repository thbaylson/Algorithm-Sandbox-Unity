using System.Linq;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial class EntityCountUISystem : SystemBase
{
    private EntityCountUI uiManager;
    private EntityQuery rotateSpeedQuery;

    protected override void OnCreate()
    {
        rotateSpeedQuery = GetEntityQuery(ComponentType.ReadOnly<RotateSpeed>());
        RequireForUpdate<RotateSpeed>();
    }

    protected override void OnStartRunning()
    {
        // Find your UI manager in the scene.
        uiManager = GameObject.FindAnyObjectByType<EntityCountUI>();

        if (uiManager == null)
        {
            Debug.LogError("EntityCountUI not found in the scene.");
        }
    }

    protected override void OnUpdate()
    {
        if (uiManager == null) return;

        // Count entities with RotateSpeed component.
        int entityCount = rotateSpeedQuery.CalculateEntityCount();

        // Update UI element.
        uiManager.UpdateCountText(entityCount);
    }
}
