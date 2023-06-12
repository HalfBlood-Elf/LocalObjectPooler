using System;
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

        public Transform Container => parent;

        protected abstract Stack<T> PoolStack { get; set; }

        public ObjectPooler(GameObject prefab, Transform parent, uint initialPoolCount = 5)
        {
            PoolStack = new();
            this.prefab = prefab;
            this.parent = parent;
            this.initialPoolCount = initialPoolCount;

            CheckForCreatedObjects();

            for (byte i = 0; i < this.initialPoolCount; i++)
            {
                ReturnToPool(InstantiatePrefab());
            }
        }

        protected abstract T InstantiatePrefab();

        public virtual T GetFreeObject(PoolableItemSetupParameters setupParameters = null)
        {
            var freeObj = PoolStack.Count > 0 ? PoolStack.Pop() : InstantiatePrefab();
            if(freeObj is ISetupable setupable) setupable.Setup(setupParameters);
            return freeObj;
        }

        public abstract void ReturnToPool(T obj);

        protected abstract void CheckForCreatedObjects();
    }
}
