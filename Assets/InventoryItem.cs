using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory/Inventory Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public string category;
    public GameObject modelPrefab;
    public bool isLocked;
    public int scale;

    public int idx = 0;
}