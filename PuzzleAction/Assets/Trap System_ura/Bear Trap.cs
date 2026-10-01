using UnityEngine;
using static EnemyData;

public class BearTrap : TrapBase
{
    [Header("Bear Trap")]
    [SerializeField] private float m_damage = 10.0f;

    [Header("Recovery")]
    [SerializeField] private float m_recoveryTime = 3.0f;

    private bool m_isActive;

    protected override void EntitySetUp()
    {
        m_isActive = true;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!m_isActive)
        {
            return;
        }

        Entity target = other.GetComponent<Entity>();

        if (target == null)
        {
            return;
        }

        m_damageData = new DamageData
        {
            Attack = m_damage,
            AttackDir =
                (target.transform.position - transform.position).normalized
        };

        target.TakeDamage(m_damageData);

        OnHit();

        m_isActive = false;

        Invoke(nameof(RecoverTrap), m_recoveryTime);
    }

    protected override void OnHit()
    {
        Debug.Log("トラばさみに引っかかった！");
    }

    private void RecoverTrap()
    {
        m_isActive = true;

        Debug.Log("トラばさみが復活しました！");
    }

    public override void TrapInit()
    {
        base.TrapInit();

        CancelInvoke(nameof(RecoverTrap));

        m_isActive = true;
    }
}