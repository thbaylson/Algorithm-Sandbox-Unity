using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Testing.ObjectPool
{
    /// <summary>
    /// Features a pool of reusable objects. Maintains size as needed. Limits the size of the pool from growing too large
    /// and removes unused objects from the pool. Prevents the accumulation of excessive instances.
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        // TODO: Figure out how to switch between poolable types. Maybe generics? A Factory?
        [SerializeField] private ImmortalPoolable objectPrefab;
        [SerializeField] public List<ImmortalPoolable> Objects {  get; private set; }

        [Header("Settings")]
        [SerializeField] private int maxNumObjects = 5;
        [SerializeField] private Vector3 objectOffset = Vector3.zero;

        private void Start()
        {
            Objects = new();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ImmortalPoolable InstantiateObject()
        {
            // Cache this value bc we'll need it a few times
            int objectCount = Objects.Count();

            // Check to see if we have an available object that's already instantiated but marked inactive
            ImmortalPoolable availableObject = Objects.Where(obj => obj.GetStatus().Equals(PoolableStatus.Inactive)).FirstOrDefault();
            if (availableObject != null)
            {
                // We have an available object, so let's activate it
                availableObject.SetStatus(PoolableStatus.Active);
                return null;
            }
            else
            {
                // There are no available objects, we want to instantiate a new one, but first let's check the limit
                if (objectCount < maxNumObjects)
                {
                    // The pool isn't full, so let's instantiate a new one and put it in the pool
                    ImmortalPoolable newObject = Instantiate(
                        objectPrefab,
                        new Vector3(
                            objectCount * objectOffset.x,
                            objectCount * objectOffset.y,
                            objectCount * objectOffset.z),
                        Quaternion.identity);

                    newObject.transform.parent = transform;
                    newObject.name = $"{objectPrefab.name} {objectCount}";
                    Objects.Add(newObject);

                    return newObject;
                }
                else
                {
                    /**
                     *  This is the case where we have no available objects and the pool is full. Oh no! There are 
                     *  various ways to handle this case. We could create a queue of objects waiting to be instantiated,
                     *  log an error message, or simply do nothing at all.
                     */
                    return null;
                }
            }
        }
    }
}