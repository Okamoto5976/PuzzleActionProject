using UnityEngine;

public class RockTrap : TrapBase
{
    //private void OnEnable()
    //{
    //    if (m_rb != null)
    //    {
    //        m_rb.linearVelocity = Vector3.zero;
    //        m_rb.angularVelocity = Vector3.zero;
    //    }
    //}

    protected override void SetUp()
    {
        if (m_rb != null)
        {
            m_rb.linearVelocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }
    }

    protected override void OnHit()
    {
        //break anim

        OnReturnPool();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") ||
        other.CompareTag("Ground"))
        {
            OnHit();
            return;
        }

        Entity target =
            other.GetComponentInParent<Entity>();

        if (target == null)
            return;

        //if (target.Team ==
        //    TeamType.Nature)
        //    return;

        if (m_owner != null)
        {
            if (target.Team ==
                m_owner.Team)
                return;
        }

        target.TakeDamage(m_damageData);

        //Debug.Log(
        //    $"{other.name} Hit");

        //Destroy(gameObject);
        OnHit();
    }

    
}
