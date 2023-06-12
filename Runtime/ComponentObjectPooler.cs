using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LocalObjectPooler
{
    public class ComponentObjectPooler<T> : ObjectPooler<T> where T : MonoBehaviour
    {
        protected Stack<T> objectsStack;
        protected override Stack<T> PoolStack { get => objectsStack; set => objectsStack = value; }

        public ComponentObjectPooler(T prefab, Transform parent, uint initialPoolCount = 5) : base(prefab.gameObject, parent, initialPoolCount)
        {
        }

        protected override T InstantiatePrefab()
        {
            var instantiated = Object.Instantiate(prefab, parent).GetComponent<T>();
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
            obj.transform.SetParent(parent);
            PoolStack.Push(obj);
        }

        protected override void CheckForCreatedObjects()
        {
            var listToDestroy = new List<GameObject>();
            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                var inScene = child.GetComponent<T>();
                if (inScene)
                {
                    ReturnToPool(inScene);
                    if (initialPoolCount > 0) initialPoolCount--;
                }
                else listToDestroy.Add(child.gameObject);
            }
            for (int i = 0; i < listToDestroy.Count; i++)
            {
                Object.Destroy(listToDestroy[i]);
            }
        }
    }
}
