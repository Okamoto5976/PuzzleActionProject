using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/Datas/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("ˆÚ“®Ý’è")]
    private float m_moveSpeed = 3f; //ˆÚ“®‘¬“x‚ÌÝ’è
    [Header("UŒ‚Ý’è")]
    private  AttackType m_attackType = AttackType.HitCollider;
    [Header("‘Ì—ÍÝ’è")]
    private int m_hp = 100;


    public float MoveSpeed { get => m_moveSpeed; }
    public AttackType MoveAttack { get => m_attackType; }
    public int HP { get => m_hp; }


    public enum AttackType
    {
        HitCollider, //‹ß‹——£
        RayCollider  //‰“‹——£
    }
     
}
