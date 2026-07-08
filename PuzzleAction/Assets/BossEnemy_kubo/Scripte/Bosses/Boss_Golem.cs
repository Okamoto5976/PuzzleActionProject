using UnityEngine;

[RequireComponent(typeof(BossGolemAttack))]
public class Boss_Golem : MonoBehaviour, IBossBehaviour
{
    [Header("Attack Range")]
    [SerializeField] private float m_shortRange = 4f;
    [SerializeField] private float m_longRange = 12f;

    private BossEnemyController m_controller;
    private BossGolemAttack m_attack;

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
            return;

        float distance =
            Vector3.Distance(
                transform.position,
                m_controller.Target.Value
            );

        // çUåÇâ¬î\ãóó£Ç»ÇÁçUåÇ
        if (distance <= m_longRange)
        {
            Attack();
            return;
        }

        // îÕàÕäOÇ»ÇÁí«ê’
        m_controller.SetDestination(
            m_controller.Target.Value,
            m_controller.Speed
        );
    }

    public void Attack()
    {
        if (m_controller.IsAttacking)
            return;

        float distance =
            Vector3.Distance(
                transform.position,
                m_controller.Target.Value
            );

        Enum_GolemAttackType attackType;

        if (distance <= m_shortRange)
        {
            attackType = Enum_GolemAttackType.Stomp;
        }
        else
        {
            attackType = Enum_GolemAttackType.RockThrow;
        }

        m_controller.StartAttack();

        switch (attackType)
        {
            case Enum_GolemAttackType.RockThrow:
                m_attack.StartRockThrow();
                break;

            case Enum_GolemAttackType.Stomp:
                m_attack.StartStomp();
                break;

            case Enum_GolemAttackType.Rush:
                m_controller.EndAttack();
                break;

            case Enum_GolemAttackType.Spin:
                m_controller.EndAttack();
                break;
        }
    }

    public void Stop()
    {
        m_controller.Stop();
    }
}