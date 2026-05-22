using UnityEngine;

[CreateAssetMenu(menuName = "Trap/RockData")] 

public class RockData : ScriptableObject
{
    public float Speed = 10f;

    public float range = 10f;

    public int damage = 10;
}
