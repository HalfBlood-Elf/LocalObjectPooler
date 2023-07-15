using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LocalObjectPooler.Samples
{
    public class SampleGridParent : MonoBehaviour
    {
        [SerializeField] private byte _startCount;
        [SerializeField] private SampleItem _prefab;
        [SerializeField] private Transform _parent;
        [SerializeField] private Button _addElementButton;
        [SerializeField] private Button _removeLastElementButton;
        
        private ComponentObjectPooler<SampleItem> _pooler;
        private readonly List<SampleItem> _activeItems = new();

        private void OnValidate()
        {
            if(!_prefab) return;
            if(!_parent) return;
            
            SetupElements(_startCount);
            // found out that this piece of code makes a warning
            // "SendMessage cannot be called during Awake, CheckConsistency, or OnValidate (Content: OnTransformChildrenChanged)"
            // because of changing child index
        }

        private void Start()
        {
            if(_addElementButton) _addElementButton.onClick.AddListener(AddElement);
            if(_removeLastElementButton) _removeLastElementButton.onClick.AddListener(RemoveLastElement);
            SetupElements(_startCount);
        }

        private void SetupElements(int elementsCount)
        {
            _pooler ??= new(new Factory<SampleItem>(_prefab, _parent, false));
            var difference = elementsCount - _activeItems.Count;
            if (difference == 0) return;
            var needToAdd = difference > 0;
            var needToRemove = difference < 0;
            for (int i = 0; i < Mathf.Abs(difference); i++)
            {
                if (needToAdd) AddElement();
                else if (needToRemove) RemoveLastElement();
            }
        }

        private void AddElement()
        {
            var item = _pooler.GetFreeObject(new SampleItem.SetupParameters { Index = _activeItems.Count });
            _activeItems.Add(item);
            item.transform.SetSiblingIndex(_activeItems.Count-1);
        }
        
        private void RemoveLastElement()
        {
            if (!_activeItems.Any()) return;
            var item = _activeItems.Last();
            _pooler.ReturnToPool(item);
            _activeItems.Remove(item);
        }
    }
}
