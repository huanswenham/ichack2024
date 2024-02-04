using System;
using UnityEngine;

public class GameState
{
    private GameState() {}

    private InventoryItem ObjPrefab;

    private static Lazy<GameState> LazyGameState = new(() => new GameState()); 
    public static GameState GetInstance {
        get{
            return LazyGameState.Value;
        }
    }

    public InventoryItem getObjPrefab() {
        return ObjPrefab;
    }

    public void setObjPrefab(InventoryItem obj) {
        ObjPrefab = obj;
    }
}
