using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LocalObjectPooler
{
    public class GameObjectPooler : ObjectPooler<GameObject>
    {
        protected override Stack<GameObject> PoolStack { get; } = new();

        public GameObjectPooler(GameObject prefab, Transform parent, uint initialPoolCount = 5) : base(prefab, parent, initialPoolCount)
        {
        }

        protected override GameObject InstantiatePrefab()
        {
            var instantiated = Object.Instantiate(Prefab, Parent);
            instantiated.SetActive(false);
            return instantiated;
        }

        public override void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);
            PoolStack.Push(obj);
        }

        public override void CheckForCreatedObjects(Transform parentToCheck)
        {
            foreach (Transform child in parentToCheck)
            {
                ReturnToPool(child.gameObject);
                if (InitialPoolCount > 0) InitialPoolCount--;
            }
        }
    }
}
