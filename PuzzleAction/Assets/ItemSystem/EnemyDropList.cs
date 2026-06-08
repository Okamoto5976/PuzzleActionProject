using System.Collections.Generic;
using UnityEngine;

public class EnemyDropList : ScriptableObject
{
    [SerializeField]public string EnemyName; 
    public List <ItemData> DropList =new List <ItemData> ();

}
