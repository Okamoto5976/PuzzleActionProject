using UnityEngine;

[CreateAssetMenu(menuName = "Trap/RockData")] 

public class RockData : ScriptableObject
{
    public float Speed = 5f;

    public int damage = 20;

    public float range = 10f;
}
