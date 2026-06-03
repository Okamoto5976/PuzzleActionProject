using UnityEngine;

[CreateAssetMenu(fileName = "RockData" ,menuName = "Trap/RockData")] 

public class RockData : ScriptableObject
{
    public int damage = 10;

    public float range = 10f;
}
