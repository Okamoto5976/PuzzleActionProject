using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveData")]
public class SaveData2 : ScriptableObject
{
    public List<SaveItemData> activeItems = new();
    public List<SaveItemData> passiveItems = new();
}