using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "dropTable", menuName = "Scriptable Objects/dropTable")]
public class dropTable : ScriptableObject
{


    public List<Item> dropItems;
    public ItemData[] possibleItems;
}
