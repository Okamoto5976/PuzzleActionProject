using UnityEngine;

public class DroppedItem : DroppedObject
{
    [SerializeField] private ItemData m_itemData;

    /// <summary>
    /// example function
    /// </summary>
    public void PrintData()
    {
        Debug.Log($"ITEM : {m_itemData.ItemName}");
    }
}
