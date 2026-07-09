using UnityEngine;

[CreateAssetMenu(menuName = "Trap/ArrowData")]

public class ArrowData : ScriptableObject
{
    [Header("Status")]

    public float speed = 10f;

    public float range = 10f;

    public int damage = 10;
}
