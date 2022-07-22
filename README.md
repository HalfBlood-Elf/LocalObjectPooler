# LocalObjectPooler
UnityPackage for object pooling

# Installing
Please reffer to [Package Manager installation instructions](https://docs.unity3d.com/Manual/upm-ui-giturl.html)

# Sample
```c#
using UnityEngine;
using UnityEngine.UI;

public class Item: MonoBehaviour
{
    [SerializeField] private Text displayText;
    public void Setup(string text) => displayText.text = text;
}

public class ItemContainer: MonoBehaviour
{
    [SerializeField] private Item itemPrefab;
    [SerializeField] private Transform container; //container better have layout group
    
    private ComponentObjectPooler<Item> itemPooler;
    private List<Item> activeItems = new();
    private void Start()
    {
        itemPooler = new(itemPrefab, container);
    }
    
    public void OnAddItemButton()
    {
        var item = itemPooler.GetFreeObject();
        item.Setup(activeItems.Count.ToString());
        item.gameObject.SetActive(true);
        activeItems.Add(item);
    }    
    
    public void OnRemoveItemButton()
    {
        var item = activeItems[^1];
        itemPooler.ReturnToPool(item);
        activeItems.RemoveAt(activeItems.Count-1);
    }
}

```
