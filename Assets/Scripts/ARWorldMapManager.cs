using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Json;
using System.Text;
#if UNITY_IOS
using System.Collections;
using Unity.Collections;
using UnityEngine.XR.ARKit;
#endif

namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// Demonstrates saving and loading an
    /// <a href="https://developer.apple.com/documentation/arkit/arworldmap">ARWorldMap</a>.
    /// </summary>
    public class ARWorldMapManager : MonoBehaviour
    {
        List<string> m_LogMessages;

        [Tooltip("The ARSession component controlling the session from which to generate ARWorldMaps.")]
        [SerializeField]
        ARSession m_ARSession;

        [Tooltip("UI Text component to display error messages")]
        [SerializeField]
        Text m_ErrorText;

        [Tooltip("The UI Text element used to display log messages.")]
        [SerializeField]
        Text m_LogText;

        [Tooltip("The UI Text element used to display the current AR world mapping status.")]
        [SerializeField]
        Text m_MappingStatusText;

        [Tooltip("A UI button component which will generate an ARWorldMap and save it to disk.")]
        [SerializeField]
        Button m_SaveButton;

        [Tooltip("A UI button component which will load a previously saved ARWorldMap from disk and apply it to the current session.")]
        [SerializeField]
        Button m_LoadButton;

        [SerializeField]
        private GameObject ARWorldMapSpawner;

        static string path => Path.Combine(Application.persistentDataPath, "my_session.worldmap");
        static string prefabPath => Path.Combine(Application.persistentDataPath, "my_session.txt");

        bool supported
        {
            get
            {
#if UNITY_IOS
                return m_ARSession.subsystem is ARKitSessionSubsystem && ARKitSessionSubsystem.worldMapSupported;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Create an <c>ARWorldMap</c> and save it to disk.
        /// </summary>
        public void OnSaveButton()
        {
#if UNITY_IOS
            StartCoroutine(Save());
#endif
        }

        /// <summary>
        /// Load an <c>ARWorldMap</c> from disk and apply it to the current session.
        /// </summary>
        public void OnLoadButton()
        {
#if UNITY_IOS
            StartCoroutine(Load());
#endif
        }

        /// <summary>
        /// Reset the <c>ARSession</c>, destroying any existing trackables, such as planes.
        /// Upon loading a saved <c>ARWorldMap</c>, saved trackables will be restored.
        /// </summary>
        public void OnResetButton()
        {
            m_ARSession.Reset();
        }

#if UNITY_IOS
        IEnumerator Save()
        {
            var sessionSubsystem = (ARKitSessionSubsystem)m_ARSession.subsystem;
            if (sessionSubsystem == null)
            {
                Log("No session subsystem available. Could not save.");
                yield break;
            }

            var request = sessionSubsystem.GetARWorldMapAsync();

            while (!request.status.IsDone())
                yield return null;

            if (request.status.IsError())
            {
                Log($"Session serialization failed with status {request.status}");
                yield break;
            }

            var worldMap = request.GetWorldMap();
            request.Dispose();

            SaveAndDisposeWorldMap(worldMap);
            SavePrefabsData();
            Log($"Successfully written to {prefabPath}!");

            NetworkManager.UploadFile(path, "./rooms/my_session.worldmap");
            NetworkManager.UploadFile(prefabPath, "./rooms/my_session.txt");
        }

        void SaveAndDisposeWorldMap(ARWorldMap worldMap)
        {
            // Log("Serializing ARWorldMap to byte array...");
            var data = worldMap.Serialize(Allocator.Temp);
            // Log($"ARWorldMap has {data.Length} bytes.");

            var file = File.Open(path, FileMode.Create);
            var writer = new BinaryWriter(file);
            writer.Write(data.ToArray());
            writer.Close();
            data.Dispose();
            worldMap.Dispose();
            // Log($"ARWorldMap written to {path}");
        }

        void SavePrefabsData() {
            string prefabsData = ARWorldMapSpawner.GetComponent<ARWorldMapSpawner>().GetSavedSpawnsAsString();
            Log(prefabsData);
            // Dictionary<int, int> test = new Dictionary<int, int> {{123, 1}, {321, 2}};
            WriteToFile(prefabPath, prefabsData);
        }

        // static string SerializeDictionary(Dictionary<int, int> dictionary) {
        //     using (MemoryStream memoryStream = new MemoryStream())
        //     {
        //         DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Dictionary<int, int>));
        //         jsonSerializer.WriteObject(memoryStream, dictionary);
        //         return Encoding.Default.GetString(memoryStream.ToArray());
        //     }
        // }

        static void WriteToFile(string filePath, string data) {   
            // Use StreamWriter to write data into the file
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(data);
            }
        }

        string ReadPrefabSpawnsData() {
            return ReadFromFile(prefabPath);
        }

        // static Dictionary<int, int> DeserializeDictionary(string jsonString) {
        //     using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
        //     {
        //         DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Dictionary<int, int>));
        //         return (Dictionary<int, int>)jsonSerializer.ReadObject(memoryStream);
        //     }
        // }

        static string ReadFromFile(string filePath) {
            // Use StreamReader to read data from the file
            using (StreamReader reader = new StreamReader(filePath))
            {
                // Read the entire content of the file
                return reader.ReadToEnd();
            }
        }

        IEnumerator Load()
        {
            var sessionSubsystem = (ARKitSessionSubsystem)m_ARSession.subsystem;
            if (sessionSubsystem == null)
            {
                Log("No session subsystem available. Could not load.");
                yield break;
            }

            FileStream file;
            try
            {
                file = File.Open(path, FileMode.Open);
            }
            catch (FileNotFoundException)
            {
                Debug.LogError("No ARWorldMap was found. Make sure to save the ARWorldMap before attempting to load it.");
                yield break;
            }

            Log($"Reading {path}...");

            const int bytesPerFrame = 1024 * 10;
            var bytesRemaining = file.Length;
            var binaryReader = new BinaryReader(file);
            var allBytes = new List<byte>();
            while (bytesRemaining > 0)
            {
                var bytes = binaryReader.ReadBytes(bytesPerFrame);
                allBytes.AddRange(bytes);
                bytesRemaining -= bytesPerFrame;
                yield return null;
            }

            var data = new NativeArray<byte>(allBytes.Count, Allocator.Temp);
            data.CopyFrom(allBytes.ToArray());

            Log("Deserializing to ARWorldMap...");
            if (ARWorldMap.TryDeserialize(data, out ARWorldMap worldMap))
                data.Dispose();

            if (worldMap.valid)
            {
                Log("Deserialized successfully.");
            }
            else
            {
                Debug.LogError("Data is not a valid ARWorldMap.");
                yield break;
            }

            Log("Apply ARWorldMap to current session.");
            sessionSubsystem.ApplyWorldMap(worldMap);

            string prefabsData = ReadPrefabSpawnsData();
            Log($"Successfully read prefabs data file!");
            Log(prefabsData);
            ARWorldMapSpawner.GetComponent<ARWorldMapSpawner>().SpawnAllPrefabs(prefabsData);
        }
#endif

        void Awake()
        {
            m_LogMessages = new List<string>();
        }

        void Log(string logMessage)
        {
            m_LogMessages.Add(logMessage);
        }

        static void SetActive(Button button, bool active)
        {
            if (button != null)
                button.gameObject.SetActive(active);
        }

        static void SetActive(Text text, bool active)
        {
            if (text != null)
                text.gameObject.SetActive(active);
        }

        static void SetText(Text text, string value)
        {
            if (text != null)
                text.text = value;
        }

        void Update()
        {
            if (supported)
            {
                SetActive(m_ErrorText, false);
                SetActive(m_SaveButton, true);
                SetActive(m_LoadButton, true);
                SetActive(m_MappingStatusText, true);
            }
            else
            {
                SetActive(m_ErrorText, true);
                SetActive(m_SaveButton, false);
                SetActive(m_LoadButton, false);
                SetActive(m_MappingStatusText, false);
            }

#if UNITY_IOS
            var sessionSubsystem = (ARKitSessionSubsystem)m_ARSession.subsystem;
            if (sessionSubsystem == null)
                return;

            var numLogsToShow = 20;
            string msg = "";
            for (int i = Mathf.Max(0, m_LogMessages.Count - numLogsToShow); i < m_LogMessages.Count; ++i)
            {
                msg += m_LogMessages[i];
                msg += "\n";
            }
            SetText(m_LogText, msg);
            SetText(m_MappingStatusText, $"Mapping Status: {sessionSubsystem.worldMappingStatus}");
#endif
        }
    }
}