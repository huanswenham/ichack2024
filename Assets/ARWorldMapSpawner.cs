using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARWorldMapSpawner : MonoBehaviour
{
    private readonly Dictionary<string, List<(string, string)>> SavedSpawns = new();

    [SerializeField]
    private GameObject placeablePrefab;

    // void Start() {
    //     SpawnAllPrefabs("123:((1.0,2,3)|(3,2,1))((5,7,3)|(3,2,3))\n123:((1,2,3)|(3,2,1))");
    // }


    public void SaveSpawnedObject(string id, Vector3 position, Quaternion rotation) {
        if (!SavedSpawns.ContainsKey(id)) {
            SavedSpawns.Add(id, new List<(string, string)>());
        }
        SavedSpawns[id].Add(($"({position.x},{position.y},{position.z})", $"({rotation.x},{rotation.y},{rotation.z})"));
    }

    public string GetSavedSpawnsAsString() {
        string res = "";
        foreach (string id in SavedSpawns.Keys) {
            res += $"{id}:";
            foreach ((string pos, string rot) in SavedSpawns[id]) {
                res += $"({pos}|{rot})";
            }
            res += "\n";
        }
        return res;
    }

    public void SpawnAllPrefabs(string prefabsData) {
        string[] lines = prefabsData.Split('\n');
        foreach(string line in lines) {
            string[] parts = line.Split(new char[] { ':' }, 2, StringSplitOptions.None);
            string poses = parts[1];
            while (poses.Length > 0) {
                poses = GetSubstring(poses, 1, poses.Length - 1);
                Debug.Log(poses);
                // string[] data = poses.Split(new char[] { ')' }, 2, StringSplitOptions.None);
                string[] posRemain = poses.Split(new char[] { '|' }, 2, StringSplitOptions.None);
                // Debug.Log(posRot[0]);
                // Debug.Log(posRot[1]);
                string pos = posRemain[0];
                string posData = GetSubstring(pos, 1, pos.Length - 2);
                string[] posNums = posData.Split(',');
                Vector3 posVec = new Vector3(float.Parse(posNums[0]), float.Parse(posNums[1]), float.Parse(posNums[2]));

                string remain = posRemain[1];
                string[] rotRemain = remain.Split(new char[] { ')' }, 3, StringSplitOptions.None);
                string rotData = GetSubstring(rotRemain[0], 1, rotRemain[0].Length - 1);
                string[] rotNums = rotData.Split(new char[] { ',' }, 3, StringSplitOptions.None);
                Vector3 rotVec = new Vector3(float.Parse(rotNums[0]), float.Parse(rotNums[1]), float.Parse(rotNums[2]));

                GameObject.FindGameObjectWithTag("DebugLogger").GetComponent<DebugManager>().PrintDebug($"Spawning: {posVec}, {rotVec}");                
                Instantiate(placeablePrefab, posVec, Quaternion.Euler(rotVec));
                GameObject.FindGameObjectWithTag("DebugLogger").GetComponent<DebugManager>().PrintDebug($"Spawed!");                

                poses = rotRemain[2];
            }
        }
    }

    static string GetSubstring(string input, int startIndex, int length)
    {
        // Check for valid indices
        if (startIndex < 0 || length < 0 || startIndex + length > input.Length)
        {
            throw new ArgumentException("Invalid indices specified.");
        }

        return input.Substring(startIndex, length);
    }
}
