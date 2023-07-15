using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LocalObjectPooler.Samples
{
    public class SampleItem : MonoBehaviour, IPoolableItem
    {
        [SerializeField] private TextMeshProUGUI _text;
        public void Setup(PoolableItemSetupParameters poolableItemSetupParameters)
        {
            if (poolableItemSetupParameters is not SetupParameters setupParameters)
            {
                Debug.LogError($"{GetType()} has no suitable setup parameters");
                return;
            }

            _text.text = setupParameters.Index.ToString();
        }

        public void OnReturnToPool()
        {
        }
        
        public class SetupParameters: PoolableItemSetupParameters
        {
            public int Index;
        }
    }
}
