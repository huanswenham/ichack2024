using System;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    private GameState() {}

    private InventoryItem ObjPrefab;

    private List<InventoryItem> AllItems;

    private int ActiveItemIdx;

    // public Dictionary<string, List<(Vector3, Quaternion)>> Memory = new();

    private static Lazy<GameState> LazyGameState = new(() => new GameState()); 
    public static GameState GetInstance {
        get {
            return LazyGameState.Value;
        }
    }

    public InventoryItem getObjPrefab() {
        return ObjPrefab;
    }

    public void SetObjPrefab(InventoryItem obj) {
        // ActiveItemIdx = idx;
        ObjPrefab = obj;
    }

    public void SaveAvailableItems(List<InventoryItem> items) {
        AllItems = items;
    }

    public List<InventoryItem> GetAllAvailableItems() {
        return AllItems;
    }

    public int GetActivePrefabIdx() {
        return ActiveItemIdx;
    }
}
