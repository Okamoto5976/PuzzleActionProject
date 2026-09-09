using UnityEngine;

[RequireComponent(typeof(BossSlimeKingeAttack))]
public class BossEnemy_SlimeKing : MonoBehaviour ,IBossBehaviour
{
    [Header("Attack Range")]
    [SerializeField] private float m_shortRange = 4f;
    [SerializeField] private float m_longRange = 12f;

    private BossEnemyController m_controller;
    private BossSlimeKingeAttack m_attack;
    private float Distance => Vector3.Distance(transform.position, m_controller.Target.Value);

    public void Initialize(BossEnemyController controller)
    {
        m_controller = controller;
        m_attack = GetComponent<BossSlimeKingeAttack>();
        if (m_attack == null)
        {
            m_attack = gameObject.AddComponent<BossSlimeKingeAttack>();
        }
        m_attack.Initalize(controller);
    }
    public void Execute()
    {
        if (m_controller.Target == null)
            return;
        if(m_controller.IsAttacking)
        {
            Debug.Log("[SlimeKing] Attacking...");
            return;
        }
        float distance = Vector3.Distance(transform.position, m_controller.Target.Value);
        Debug.Log($"[SlimeKing]Distance: {Distance:F2}");
        if (Distance <= m_longRange)
        {
            Debug.Log("[SlimeKing] Attack Range Enter");
            Attack();
            return;
        }
        Debug.Log("[SlimeKing] Chase Player");
    }
    public void Attack()
    {
        if (m_controller.IsAttacking)
            return;
        float distance = Vector3.Distance(transform.position, m_controller.Target.Value);
        
        Enem_SlimeKingAttackType attackType;

        if( Distance <= m_shortRange)
        {
            attackType = Enem_SlimeKingAttackType.Summon;
        }
        else
        {
            attackType = Enem_SlimeKingAttackType.Tackle;
        }
      //  Debug.Log($"[SlimeKing] AttackType: {attackType}");

        m_controller.StartAttack();

        switch (attackType)
        {
            case Enem_SlimeKingAttackType.Summon:
                //m_attack.StartSummon();
                break;
            case Enem_SlimeKingAttackType.Tackle:
                //m_attack.StartTackle();
                break;
            case Enem_SlimeKingAttackType.Stamp:
                //m_attack.StartStamp();
                break;
            case Enem_SlimeKingAttackType.Rush:
                //m_attack.StartRush();
                break;
        }
    }
    public void Stop()
    {
        m_controller.Stop();
    }


}
