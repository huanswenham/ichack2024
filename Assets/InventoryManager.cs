using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    public List<InventoryItem> allItems;
    public List<InventoryItem> playerInventory;
    public GridLayoutGroup gridLayout;

    void Start()
    {
    }

    public void UnlockItem(InventoryItem item)
    {
        item.isLocked = false;
        playerInventory.Add(item);
    }
}
