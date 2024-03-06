using System.Collections;
using UnityEngine;

namespace Testing.ObjectPool
{
    /// <summary>
    /// This is the simplest example of a concrete implementation of IPoolable. The object has a defined lifeTime and after
    /// that many seconds sets itself as inactive.
    /// </summary>
    public class Poolable : MonoBehaviour, IPoolable
    {
        [Header("Settings")]
        [SerializeField] private float lifeTime = 5f;

        private PoolableStatus _status = PoolableStatus.Inactive;

        private void OnEnable()
        {
            _status = PoolableStatus.Active;
            StartCoroutine(LifeCountdownCoroutine());
        }

        public PoolableStatus GetStatus()
        {
            return _status;
        }

        public void SetStatus(PoolableStatus status)
        {
            _status = status;
            gameObject.SetActive(_status.Equals(PoolableStatus.Active));
        }

        private IEnumerator LifeCountdownCoroutine()
        {
            yield return new WaitForSeconds(lifeTime);
            SetStatus(PoolableStatus.Inactive);
        }
    }
}