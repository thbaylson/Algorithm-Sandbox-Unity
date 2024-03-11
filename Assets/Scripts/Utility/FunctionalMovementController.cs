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
                objMover.PlaceAtInitialPosition();
                objMover.SetIsMoving(true);

                if(period == 0f)
                    period = objMover.Period;
            }

            // This allows objects to be evenly spaced.
            yield return new WaitForSeconds(period / maxObjects);
        }
    }
}
