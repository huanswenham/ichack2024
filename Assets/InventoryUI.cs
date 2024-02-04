using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GridLayoutGroup gridLayout;
    public GameObject inventoryItemPrefab;
    // public InventoryItem inventoryPrefab;
    // public List<InventoryItem> inventoryListPrefab;
    public InventoryManager inventoryManager;

    void Start()
    {
        UpdateInventoryUI();
    }

    void UpdateInventoryUI()
    {
        foreach (var item in inventoryManager.allItems)
        // for (int i = 0; i < inventoryManager.allItems.Count; i++)
        {
            GameObject inventoryItem = Instantiate(inventoryItemPrefab, gridLayout.transform);
            // GameObject inventoryItem = Instantiate(inventoryListPrefab[i], gridLayout.transform);
            
            // inventoryItem.GetComponentInChildren<GameObject>() = item.modelPrefab;
            //inventoryItem.GetComponentInChildren<Text>().text = item.itemName;
            inventoryItem.GetComponent<Button>().onClick.AddListener(() => OnItemClick(item));
            // inventoryItem.GetComponent<Button>().onClick.AddListener(() => OnItemClick(inventoryListPrefab[i]));
        }
    }

    void OnItemClick(InventoryItem item)
    {
        inventoryPanel.SetActive(false);
        PlaceItemInAR(item);
    }

    void PlaceItemInAR(InventoryItem item)
    {

    }
}
