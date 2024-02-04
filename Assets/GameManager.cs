using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void OpenInventory() {
        SceneManager.LoadScene("Inventory");
    }

    public void CloseInventory() {
        SceneManager.LoadScene("Test");
    }
}
