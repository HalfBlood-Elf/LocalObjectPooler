
using UnityEngine;

namespace LocalObjectPooler
{
    public interface IFactory<T> where T: Object
    {
        T Prefab { get; set; }

        Transform Parent { get; }
        public T CreateObject();
    }
}
