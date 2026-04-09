using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData",menuName = "Enemy/Data")]
public class EnmeyData:ScriptableObject
{
    [Header("ˆÚ“®İ’è")]
    [Header("UŒ‚İ’è")]

    //ˆÚ“®İ’è
    public float moveSpeed = 0f;
    //UŒ‚İ’è
    public AttackType attackType;

    public enum AttackType
    {
        None,
    }
}
