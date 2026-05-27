using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Save/SaveData")]
public class SaveSO : ScriptableObject
{
    [System.Serializable] public class ItemSaveData
        {
            public int itemID;
            public int count;
            public int slotID;  //ƒCƒ“ƒxƒ“ƒgƒŠ
        }
        public List<ItemSaveData> items = new List<ItemSaveData>();
    public void AddItem(int itemID)
    {
        var item = items.Find(i => i.itemID == itemID);
        if (item != null)
        {
            item.count++;
        }
        else
        {
            ItemSaveData newItem = new ItemSaveData();
            newItem.itemID = itemID;
            newItem.count = 1;
            newItem.slotID = GetNextSlotID();
            items.Add(newItem);
        }
    }
    private int GetNextSlotID()
    {
        return items.Count;
    }
}
