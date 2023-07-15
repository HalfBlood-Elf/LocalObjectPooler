using UnityEditor;
using UnityEngine;

namespace LocalObjectPooler
{
    public class Factory<T>: IFactory<T> where T : Object
    {
        public T Prefab { get; set; }
        public Transform Parent { get; }
        
        public Factory(T prefab, Transform parent, bool useEditorInstantiate = true)
        {
            Prefab = prefab;
            Parent = parent;
        }


        public T CreateObject()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying && Application.isEditor) return InstantiateInEditor();
#endif
            return Object.Instantiate(Prefab, Parent);
        }

#if UNITY_EDITOR
        private T InstantiateInEditor()
        {
            return (T)PrefabUtility.InstantiatePrefab(Prefab, Parent);
        }
#endif
    }
}