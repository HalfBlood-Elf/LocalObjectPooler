using System.Collections.Generic;
using UnityEngine;

namespace LocalObjectPooler
{
    public abstract class ObjectPooler<T>
    {
        protected uint InitialPoolCount;
        protected readonly GameObject Prefab;
        protected readonly Transform Parent;

        public Transform Container => Parent;

        protected abstract Stack<T> PoolStack { get; }

        public ObjectPooler(GameObject prefab, Transform parent, uint initialPoolCount = 5)
        {
            Prefab = prefab;
            Parent = parent;
            InitialPoolCount = initialPoolCount;

            CheckForCreatedObjects(parent);

            CreateInitialPoolObjects();
        }

        private void CreateInitialPoolObjects()
        {
            for (byte i = 0; i < InitialPoolCount; i++)
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

        public abstract void CheckForCreatedObjects(Transform parentToCheck);
    }
}
