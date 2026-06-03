using UnityEngine;

[CreateAssetMenu(fileName ="TrapData",menuName = "Scriptable Objects/Datas/TrapData")]

public class TrapData:ScriptableObject
{
    public int damage = 10;

    public float range = 10f;
}
