using System.Collections.Generic;
using UnityEngine;

namespace LocalObjectPooler
{
    public class ComponentObjectPooler<T> : ObjectPooler<T> where T : MonoBehaviour
    {
        protected override Stack<T> PoolStack { get; } = new();
        
        [System.Obsolete("Please, use constructor with factory instead")]
        public ComponentObjectPooler(T prefab, Transform parent, uint initialPoolCount = 5) : base(prefab, parent, initialPoolCount)
        {
        }

        public ComponentObjectPooler(IFactory<T> factory, uint initialPoolCount = 5) : base(factory, initialPoolCount)
        {
        }

        protected override T InstantiatePrefab()
        {
            var instantiated = base.InstantiatePrefab();
            instantiated.gameObject.SetActive(false);
            return instantiated;
        }

        public override T GetFreeObject(PoolableItemSetupParameters setupParameters = null)
        {
            var freeObj = base.GetFreeObject(setupParameters);
            freeObj.gameObject.SetActive(true);
            return freeObj;
        }

        public override void ReturnToPool(T obj)
        {
            if (!obj) return;
            if (obj is IReturnableToPool returnableToPool) returnableToPool.OnReturnToPool();
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(Parent);
            PoolStack.Push(obj);
        }

        public override void CheckForCreatedObjects(Transform parentToCheck)
        {
            foreach (Transform child in parentToCheck)
            {
                if(!child.TryGetComponent(out T inScene)) Object.Destroy(child.gameObject);
                ReturnToPool(inScene);
                if (InitialPoolCount > 0) InitialPoolCount--;
            }
        }
    }
}
