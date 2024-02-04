using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class SpawnObjectOnPlane : MonoBehaviour
{

    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;
    private List<GameObject> placedPrefabList = new List<GameObject>();

    [SerializeField]
    private int maxPrefabSpawnCount = 0;
    private int placedPrefabCount;

    [SerializeField]
    private GameObject placeablePrefab;

    [SerializeField]
    private GameObject DebugLogger;

    [SerializeField]
    private GameObject ARWorldMapSpawner;
    
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private float scale = 0.1f;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Start() {
        InventoryItem item = GameState.GetInstance.getObjPrefab();
        if (item != null) {
            placeablePrefab = item.modelPrefab;
            ShowDebug("Loaded new model");
        }
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private void Update()
    {
        if(!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        if(raycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_Hits[0].pose;
            ARRaycastHit hit = s_Hits[0];
            // ShowDebug($"plane hit: {hit.trackableId}");
            if (placedPrefabCount < maxPrefabSpawnCount)
            {
                // ShowDebug($"plane id: {hitPose.GetHashCode()}");
                SpawnPrefab(hit.trackableId.ToString(), hitPose);
            }
        }
    }

    public void SetPrefabType(GameObject prefabType)
    {
        placeablePrefab = prefabType;
    }

    private void SpawnPrefab(string id, Pose hitPose)
    {
        spawnedObject = Instantiate(placeablePrefab, hitPose.position, hitPose.rotation);
        spawnedObject.transform.localScale = new Vector3(scale, scale, scale);
        ARWorldMapSpawner.GetComponent<ARWorldMapSpawner>().SaveSpawnedObject(id, hitPose.position, hitPose.rotation);
        placedPrefabList.Add(spawnedObject);
        placedPrefabCount++;
    }


    private void ShowDebug(string log) {
        DebugLogger.GetComponent<DebugManager>().PrintDebug(log);
    }
}
