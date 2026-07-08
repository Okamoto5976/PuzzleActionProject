using UnityEngine;

[RequireComponent(typeof(BossGolemAttack))]
public class Boss_Golem : MonoBehaviour, IBossBehaviour
{
    [Header("Attack Range")]
    [SerializeField] private float m_shortRange = 4f;
    [SerializeField] private float m_longRange = 12f;

    private BossEnemyController m_controller;
    private BossGolemAttack m_attack;

    private float Distance => Vector3.Distance(transform.position, m_controller.Target.Value);

    public void Initialize(BossEnemyController controller)
    {
        m_controller = controller;
        m_attack = GetComponent<BossGolemAttack>();

        if (m_attack == null)
        {
            m_attack = gameObject.AddComponent<BossGolemAttack>();
        }

        m_attack.Initialize(controller);
    }

    public void Execute()
    {
        if (m_controller.Target == null)
            return;

        if (m_controller.IsAttacking)
        {
            Debug.Log("[GOLEM] Attacking...");
            return;
        }

        float distance = Vector3.Distance(transform.position, m_controller.Target.Value );
        Debug.Log($"[Golem]Distance: {Distance:F2}");
        //Attack
        if (Distance <= m_longRange)
        {
            Debug.Log("[Golem] Attack Range Enter");
            Attack();
            return;
        }

        Debug.Log("[GOLEM] Chase Player");
        //distination
        m_controller.SetDestination(m_controller.Target.Value, m_controller.Speed );
    }

    public void Attack()
    {
        if (m_controller.IsAttacking)
            return;

        float distance = Vector3.Distance(transform.position, m_controller.Target.Value);

        Enum_GolemAttackType attackType;

        if (Distance <= m_shortRange)
        {
            attackType = Enum_GolemAttackType.Stomp;
        }
        else
        {
            attackType = Enum_GolemAttackType.RockThrow;
        }

        Debug.Log($"[GOLEM] Select Attack : {attackType}");

        m_controller.StartAttack();


        switch (attackType)
        {
            case Enum_GolemAttackType.RockThrow:

                Debug.Log("[GOLEM] Start Rock Throw");

                m_attack.StartRockThrow();
                break;

            case Enum_GolemAttackType.Stomp:

                Debug.Log("[GOLEM] Start Stomp");

                m_attack.StartStomp();
                break;

            //case Enum_GolemAttackType.Rush:

            //    Debug.Log("[GOLEM] Rush Not Implemented");

            //    m_controller.EndAttack();
            //    break;

            //case Enum_GolemAttackType.Spin:

            //    Debug.Log("[GOLEM] Spin Not Implemented");

            //    m_controller.EndAttack();
            //    break;
        }

    }

    public void Stop()
    {
        m_controller.Stop();
    }



    //-----------------------------------------------------------------------------------------
    // debug display
    //-----------------------------------------------------------------------------------------
    #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            // Short Range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,m_shortRange);

            // Long Range
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position,m_longRange);
        }
    #endif
}