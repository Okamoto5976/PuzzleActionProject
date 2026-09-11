using UnityEngine;

public class DroppedItem : DroppedObject
{
    [SerializeField] private ItemData m_itemData;

    public void PrintData()
    {
        Debug.Log($"ITEM : {m_itemData.ItemName}");
    }
}
