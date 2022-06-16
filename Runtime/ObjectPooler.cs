using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LocalObjectPooler
{
    public abstract class ObjectPooler<T>
    {
        protected uint initialPoolCount = 5;
        protected GameObject prefab;
        protected Transform parent;

        protected abstract Stack<T> PoolStack { get; set; }

        public ObjectPooler(GameObject prefab, Transform parent, uint initialPoolCount = 5)
        {
            PoolStack = new();
            this.prefab = prefab;
            this.parent = parent;
            this.initialPoolCount = initialPoolCount;

            CheckForCreatedObjects();

            for (byte i = 0; i < initialPoolCount; i++)
            {
                InstantiatePrefab();
            }
        }

        protected abstract T InstantiatePrefab();

        public virtual T GetFreeObject()
        {
            if (PoolStack.Count > 0)
            {
                return PoolStack.Pop();
            }
            else
            {
                return InstantiatePrefab();
            }
        }

        public abstract void ReturnToPool(T obj);

        protected abstract void CheckForCreatedObjects();
    }
}
