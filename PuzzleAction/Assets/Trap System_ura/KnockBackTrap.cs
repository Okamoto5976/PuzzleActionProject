using UnityEngine;

public class KnockBackTrap : TrapBase
{
    [Header("Life Time")]
    [SerializeField]
    private float m_lifeTime = 3f;

    private float m_timer;


    protected override void EntitySetUp()
    {
        m_timer = 0f;

        m_damageData = new DamageData
        {
            // ダメージはなし
            Attack = 0,

            AttackType = m_attackType,

            // ノックバック
            Knockback = m_owner.KnockBack,

            // ノックバック方向
            // 風の移動方向
            AttackDir = m_dir
        };
    }


    private void FixedUpdate()
    {
       
        OnMove(m_dir);

 
        m_timer += Time.fixedDeltaTime;

        if (m_timer >= m_lifeTime)
        {
            OnReturnPool();
        }
    }


    protected override void OnHit()
    {
       
    }


    protected override void OnTriggerEnter(Collider other)
    {
        Entity target =
            other.GetComponent<Entity>();

        if (target == null)
            return;

        if (target == m_owner)
            return;


        EntityHP entityHP =
            target.GetComponent<EntityHP>();

        if (entityHP == null)
            return;


        entityHP.TakeDamage(m_damageData);


        OnReturnPool();
    }
}