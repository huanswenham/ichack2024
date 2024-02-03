using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField]
    private GameObject DebugLogger;

    // Start is called before the first frame update
    void Start()
    {
        PrintDebug("Hello");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrintDebug (string log) {
        DebugLogger.GetComponent<TextMeshProUGUI>().text += "\n";
        DebugLogger.GetComponent<TextMeshProUGUI>().text += log;
    } 
}
