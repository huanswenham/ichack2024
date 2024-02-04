using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject InventoryManagerPrefab;

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    void Awake() {
        List<InventoryItem> availableItems = InventoryManagerPrefab.GetComponent<InventoryManager>().allItems;
        // GameState.
    }

    public void OpenInventory() {
        SceneManager.LoadScene("Inventory");
    }

    public void CloseInventory() {
        SceneManager.LoadScene("Test");
    }
}
