using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject InventoryManagerPrefab;

    [SerializeField]
    private InventoryItem initial;

    [SerializeField]
    private GameObject ARSession;

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    void Awake() {
        List<InventoryItem> availableItems = InventoryManagerPrefab.GetComponent<InventoryManager>().allItems;
        for (int i = 0; i < availableItems.Count; i++) {
            availableItems[i].idx = i;
        }
        GameState.GetInstance.SaveAvailableItems(availableItems);
        GameState.GetInstance.SetObjPrefab(availableItems[0]);

    }

    public void OpenInventory() {
        SceneManager.LoadScene("Inventory");
    }

    public void CloseInventory() {
        SceneManager.LoadScene("Test");
    }
}
