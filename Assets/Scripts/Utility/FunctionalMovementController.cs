using System.Collections;
using Testing.ObjectPool;
using UnityEngine;

public class FunctionalMovementController : MonoBehaviour
{
    public static FunctionalMovementController Instance { get; private set; }

    [SerializeField] private ObjectPool _objectPool;
    [SerializeField] private int maxObjects;

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
                objMover.SetFunction(FunctionOfTime.Circle);
                // This ensures that objects don't overlap due to spawnrate being a factor of their period
                objMover.SetMoveSpeed(maxObjects+1);
                //objMover.SetRadius(maxObjects);
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
