using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
    public TMP_InputField text;

    public void Play() {
        // Request for the files from the server.
        string filename = "./rooms/" + text.text + ".txt";
        // Potentially no download as new player.
        NetworkManager.DownloadFile(filename);
        SceneManager.LoadScene("Game");
    }
}
