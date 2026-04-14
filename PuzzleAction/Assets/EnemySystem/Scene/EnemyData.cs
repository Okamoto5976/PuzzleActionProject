using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("ˆÚ“®İ’è")]
    public float m_moveSpeed = 3f; //ˆÚ“®‘¬“x‚Ìİ’è
    [Header("UŒ‚İ’è")]
    public AttackType m_attackType = AttackType.HitCollider;


    public enum AttackType
    {
        HitCollider,
        RayCollider
    }
     
}
