using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Scriptable Objects/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] private ItemData[] items;

    /*public ItemData GetItemByName(string name){
        foreach (var item in items) {
            foreach (Item.ItemName == name) {
                return item;
            }
        }
        return null;
    }
    public ItemData GetItemByIndex(int index)
    {
        if (index >= 0 && index < items.Length)
        {
            return items[index];
        }
        return null;
    }*/
}

