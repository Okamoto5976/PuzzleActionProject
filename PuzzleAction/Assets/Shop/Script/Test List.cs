using System.Collections.Generic;
using UnityEngine;

public class TestList : MonoBehaviour
{
    DataManager dm;
    void Start()
    {
        dm = GetComponent<DataManager>();
        dm.data.ItemID.Add(99);
        Debug.Log("Œ»İ‚ÌƒCƒ“ƒxƒ“ƒgƒŠ");
        foreach (int id in dm.data.ItemID)
        {
            Debug.Log("ItemID : " + id);
        }
        dm.Save(dm.data);
    }
}