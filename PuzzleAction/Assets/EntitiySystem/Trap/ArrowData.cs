using UnityEngine;

[CreateAssetMenu(fileName ="ArrowData", menuName = "Trap/ArrowData")]

public class ArrowData : ScriptableObject
{
    public int damage = 10;

    public float range = 10f;
}
