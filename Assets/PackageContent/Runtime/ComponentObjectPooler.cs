using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LocalObjectPooler
{
    public class ComponentObjectPooler<T> : ObjectPooler<T> where T : MonoBehaviour
    {
        protected override Stack<T> PoolStack { get; } = new();
        
        public ComponentObjectPooler(T prefab, Transform parent, uint initialPoolCount = 5) : base(prefab.gameObject, parent, initialPoolCount)
        {
        }

        protected override T InstantiatePrefab()
        {
            var instantiated = Object.Instantiate(Prefab, Parent).GetComponent<T>();
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
