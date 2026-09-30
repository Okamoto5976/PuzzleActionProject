using UnityEngine;

[RequireComponent(typeof(ReturnObjectToPool))]
public class FireMagic : TrapBase
{
    [Header("Fire Magic Settings")]
    [SerializeField] private LayerMask m_hitLayers;
    [SerializeField] private float m_lifeTime = 5f;

    private float m_timer = 0f;

    protected override void EntitySetUp()
    {
        m_timer = 0f;

        if(m_rb!=null)
        {
            m_rb.isKinematic = false;
            m_rb.useGravity = false;
            m_rb.linearVelocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        OnMove(m_dir);
    }

    private void Update()
    {
        CheckDeadLine();

        m_timer += Time.deltaTime;
        if(m_timer>=m_lifeTime)
        {
            OnReturnPool();
        }
    }

    protected override void OnHit()
    {
        OnReturnPool();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if ((m_hitLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            OnHit();
            return;
        }

        Entity target =other.GetComponentInParent<Entity>();

        if (m_team == TeamType.Nature) return;


        if (target == null || target.Team == m_team) return;

        target.TakeDamage(m_damageData);

        if (m_trapData.m_buffSetting.Count != 0)
        {
            foreach (var buff in m_trapData.m_buffSetting)
            {
                if (buff.m_duration <= 0) continue;

                var modifier = SetModifier(buff);

                target.AddBuff(modifier, buff.m_buffID, buff.m_duration);
            }
        }

        OnHit();
    }
}
