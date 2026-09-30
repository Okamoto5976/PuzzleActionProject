using UnityEngine;
[RequireComponent(typeof(ReturnObjectToPool))]
public class Water : TrapBase
{
    [Header("WaterGun Settings")]
    [SerializeField] private float m_lifeTimer = 5f;

    [SerializeField] private LayerMask m_hitLayers;


    private bool m_isAddForceCalled=false;
    private float m_timer = 0f;

    protected override void EntitySetUp()
    {
        m_isAddForceCalled = false;
        m_timer = 0f;

        if(m_rb!=null)
        {
            m_rb.isKinematic =false;
            m_rb.useGravity = false;
            m_rb.linearVelocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }
    }
   
    private void FixedUpdate()
    {
        if (!m_isAddForceCalled)
        {
            OnAddForce(m_dir, m_speed);
            m_isAddForceCalled = true;
        }
    }
    private void Update()
    {
        CheckDeadLine();

        m_timer += Time.deltaTime;
        if(m_timer>=m_lifeTimer)
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

        Entity target=other.GetComponentInParent<Entity>();
        if (target == null) return;

        if (target.Team == TeamType.Nature) return;

        if (target.Team == m_team) return;

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