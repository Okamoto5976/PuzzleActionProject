using System.Collections;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("Trap Data")]
    [SerializeField]
    private TrapData m_trapData;

    [Header("Cooldown")]
    [SerializeField]
    private float m_cooldown = 2.0f;

    private bool m_isActive = true;


    private void OnTriggerEnter(Collider other)
    {
        if (!m_isActive)
        {
            return;
        }

        Entity entity = other.GetComponentInParent<Entity>();

        if (entity == null)
        {
            return;
        }

        // NatureˆÈŠO‚ğ‘ÎÛ‚É‚·‚é
        if (entity.Team == TeamType.Nature)
        {
            return;
        }

        DamageData damageData = new DamageData
        {
            Attack = m_trapData.m_trapAttack,
            CriticalRate = m_trapData.m_trapCriticalRate,
            CriticalDamage = m_trapData.m_trapCriticalDamage,
            BreakRate = m_trapData.m_trapBreakRate,
            Knockback = m_trapData.m_trapKnockBack,
            StunDuration = m_trapData.m_trapStunDuration
        };

        entity.TakeDamage(damageData);

        // ˆê“I‚É–³Œø‰»
        m_isActive = false;

        StartCoroutine(Cooldown());
    }


    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(m_cooldown);

        m_isActive = true;
    }
}
