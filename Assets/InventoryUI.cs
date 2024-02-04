using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryCanvas;
    public GameObject buttonWrapper;
    public GridLayoutGroup gridLayout;
    public InventoryManager inventoryManager;

    void Start()
    {
        UpdateInventoryUI();
    }

    void UpdateInventoryUI()
    {
        foreach (InventoryItem item in inventoryManager.allItems)
        {
            GameObject buttonObj = Instantiate(buttonWrapper, gridLayout.transform);
            GameObject inventoryItem = Instantiate(item.modelPrefab, buttonObj.transform);  
            inventoryItem.transform.localScale = new Vector3(item.scale, item.scale, item.scale);    
            // Vector3 rotation = inventoryItem.transform.localRotation.eulerAngles;      
            // inventoryItem.transform.Rotate(rotation.x, rotation.y, rotation.z, Space.Self);
            inventoryItem.transform.Rotate(-90, 180, 0, Space.World);
            // item.idx = i;
            //inventoryItem.GetComponentInChildren<Text>().text = item.itemName;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => OnItemClick(item));
        }
    }

    void OnItemClick(InventoryItem item)
    {
        PlaceItemInAR(item);
        SceneManager.LoadScene("Test");
    }

    void PlaceItemInAR(InventoryItem item)
    {
        GameState.GetInstance.SetObjPrefab(item);
    }
}
