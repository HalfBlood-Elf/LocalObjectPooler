using System.Collections.Generic;
using UnityEngine;

namespace LocalObjectPooler
{
    public abstract class ObjectPooler<T> where T : Object
    {
        protected uint InitialPoolCount;
        private readonly IFactory<T> _factory;
        
        public Transform Parent => _factory.Parent;

        protected abstract Stack<T> PoolStack { get; }

        [System.Obsolete("Please, use constructor with factory instead")]
        public ObjectPooler(T prefab, Transform parent, uint initialPoolCount = 5): this(new Factory<T>(prefab, parent), initialPoolCount)
        {
        }

        public ObjectPooler(IFactory<T> factory, uint initialPoolCount = 5)
        {
            _factory = factory;
            InitialPoolCount = initialPoolCount;

            CheckForCreatedObjects(Parent);

            CreateInitialPoolObjects();
        }

        private void CreateInitialPoolObjects()
        {
            for (byte i = 0; i < InitialPoolCount; i++)
            {
                ReturnToPool(InstantiatePrefab());
            }
        }

        protected virtual T InstantiatePrefab()
        {
            var instantiated = _factory.CreateObject();
            return instantiated;
        }

        public virtual T GetFreeObject(PoolableItemSetupParameters poolableItemSetupParameters = null)
        {
            var freeObj = PoolStack.Count > 0 ? PoolStack.Pop() : null; 
            // if pop() will return deleted element, or stack is empty - we will create one
            if(!freeObj) freeObj = InstantiatePrefab();
            if(freeObj is ISetupable setupable) setupable.Setup(poolableItemSetupParameters);
            return freeObj;
        }

        public abstract void ReturnToPool(T obj);

        public abstract void CheckForCreatedObjects(Transform parentToCheck);
    }
}
