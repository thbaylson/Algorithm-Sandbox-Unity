using System.Collections;
using Testing.ObjectPool;
using UnityEngine;

public class FunctionalMovementController : MonoBehaviour
{
    public static FunctionalMovementController Instance { get; private set; }

    [SerializeField] private ObjectPool _objectPool;
    [SerializeField] private int maxObjects;

    // New field for the movement function ScriptableObject.
    [SerializeField] private MovementFunction movementFunctionAsset;

    private void Start()
    {
        StartCoroutine(SpawnObjectsRoutine());
    }

    private IEnumerator SpawnObjectsRoutine()
    {
        float period = 0f;
        while (_objectPool.Objects.Count < maxObjects)
        {
            ImmortalPoolable obj = _objectPool.InstantiateObject();
            FunctionalMover objMover = obj.GetComponent<FunctionalMover>();
            if (objMover != null)
            {
                objMover.SetMovementFunction(movementFunctionAsset);
                // TODO: Find a better way of setting speed.
                //  Currently, adding more than a few objects makes the speed way too high.
                objMover.SetMoveSpeed(maxObjects+1);
                objMover.SetIsMoving(true);
                objMover.PlaceAtInitialPosition();

                if(period == 0f)
                    period = objMover.Period;
            }

            // This allows objects to be evenly spaced.
            yield return new WaitForSeconds(period / maxObjects);
        }
    }
}
