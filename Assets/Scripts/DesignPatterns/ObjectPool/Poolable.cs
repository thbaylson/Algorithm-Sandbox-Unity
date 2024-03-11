using System.Collections;
using UnityEngine;

namespace Testing.ObjectPool
{
    /// <summary>
    /// This is an example of a concrete implementation of IPoolable. The object has a defined lifeTime and after
    /// that many seconds sets itself as inactive.
    /// </summary>
    public class Poolable : ImmortalPoolable
    {
        [Header("Settings")]
        [SerializeField] private float lifeTime = 5f;

        protected override void OnEnable()
        {
            _status = PoolableStatus.Active;
            StartCoroutine(LifeCountdownCoroutine());
        }

        private IEnumerator LifeCountdownCoroutine()
        {
            yield return new WaitForSeconds(lifeTime);
            SetStatus(PoolableStatus.Inactive);
        }
    }
}