using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Testing.ObjectPool
{
    public class ImmortalPoolable : MonoBehaviour, IPoolable
    {
        protected PoolableStatus _status = PoolableStatus.Inactive;

        protected virtual void OnEnable()
        {
            _status = PoolableStatus.Active;
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
    }
}
