using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    public List<InventoryItem> allItems;
    public List<InventoryItem> playerInventory;

    void Start()
    {
    }

    public void UnlockItem(InventoryItem item)
    {
        item.isLocked = false;
        playerInventory.Add(item);
    }
}
