using UnityEngine;

public class WindMagic : TrapBase
{
    [Header("Life Time")]
    [SerializeField] private LayerMask m_hitLayers;
    [SerializeField] private float m_lifeTime = 3f;

    private float m_timer;

    public override void TrapInit()
    {
        base.TrapInit();

        m_timer = 0f;
    }

    protected override void EntitySetUp()
    {
        m_timer = 0f;
    }

    private void FixedUpdate()
    {
        OnMove(m_dir);
    }

    private void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer >= m_lifeTime)
        {
            m_timer = 0f;

            OnHit();
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


        Entity target =
            other.GetComponentInParent<Entity>();

        if (target == null) return;

        if (target.Team == TeamType.Nature) return;

        if (target.Team == m_team) return;

        target.TakeDamage(m_damageData);



    }
}