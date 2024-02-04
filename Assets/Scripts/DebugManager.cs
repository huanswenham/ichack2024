using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public void PrintDebug (string log) {
        GetComponent<TextMeshProUGUI>().text += ", ";
        GetComponent<TextMeshProUGUI>().text += log;
    } 
}
