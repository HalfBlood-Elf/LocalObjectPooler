using System.Collections.Generic;
using UnityEngine;

namespace LocalObjectPooler
{
    public class GameObjectPooler : ObjectPooler<GameObject>
    {
        protected override Stack<GameObject> PoolStack { get; } = new();

        [System.Obsolete("Please, use constructor with factory instead")]
        public GameObjectPooler(GameObject prefab, Transform parent, uint initialPoolCount = 5) : base(prefab, parent, initialPoolCount)
        {
        }
        
        public GameObjectPooler(IFactory<GameObject> factory, uint initialPoolCount = 5) : base(factory, initialPoolCount)
        {
        }

        protected override GameObject InstantiatePrefab()
        {
            var instantiated = base.InstantiatePrefab();
            instantiated.SetActive(false);
            return instantiated;
        }

        public override void ReturnToPool(GameObject obj)
        {
            if (!obj) return;
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
