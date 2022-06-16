using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LocalObjectPooler
{
    public class GameObjectPooler : ObjectPooler<GameObject>
    {
        protected Stack<GameObject> objectsStack;
        protected override Stack<GameObject> PoolStack { get => objectsStack; set => objectsStack = value; }


        public GameObjectPooler(GameObject prefab, Transform parent, uint initialPoolCount = 5) : base(prefab, parent, initialPoolCount)
        {
        }

        protected override GameObject InstantiatePrefab()
        {
            var instantiated = Object.Instantiate(prefab, parent);
            instantiated.SetActive(false);
            return instantiated;
        }

        public override void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);
            PoolStack.Push(obj);
        }

        protected override void CheckForCreatedObjects()
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);

                ReturnToPool(child.gameObject);
                if (initialPoolCount > 0) initialPoolCount--;

            }
        }
    }
}
