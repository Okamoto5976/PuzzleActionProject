using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Data[] items;

    public Data GetItem(int id)
    {
        foreach (Data item in items)
        {
            if (item.ID == id)
            {
                return item;
            }
        }
        return null;
    }
}
