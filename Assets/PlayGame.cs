using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
    public TMP_InputField text;

    public void Play() {
        // Request for the files from the server.
        string worldFile = "./rooms/my_session.worldmap";
        string prefabsFile = "./rooms/my_session.txt";
        // Potentially no download as new player.
        NetworkManager.DownloadFile(worldFile, Path.Combine(Application.persistentDataPath, "my_session.worldmap"));
        NetworkManager.DownloadFile(prefabsFile, Path.Combine(Application.persistentDataPath, "my_session.txt"));
        SceneManager.LoadScene("Test");
    }
}
