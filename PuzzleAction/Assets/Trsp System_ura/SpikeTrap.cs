using UnityEngine;

public class SpikeTrap : TrapBase
{
    [Header("Damage")]
    [SerializeField]
    private Collider m_damageCollider;


    private bool m_isAttacking;
    private bool m_isActivated;


    protected override void SetUp()
    {
        // 初期化
        m_damageCollider.enabled = false;

        m_isAttacking = false;
        m_isActivated = false;
    }


    // Playerを検知した時に呼ぶ
    public void Activate()
    {
        if (m_isActivated)
        {
            return;
        }

        m_isActivated = true;
    }


    // 針が攻撃可能な状態になった時に呼ぶ
    public void StartDamage()
    {
        if (!m_isActivated)
        {
            return;
        }

        m_isAttacking = true;
        m_damageCollider.enabled = true;
    }


    // 針が引っ込む時に呼ぶ
    public void EndDamage()
    {
        m_isAttacking = false;
        m_damageCollider.enabled = false;
    }


    protected override void OnHit()
    {
        Debug.Log("針トラップが命中！");
    }


    protected override void OnTriggerEnter(
        Collider other)
    {
        if (!m_isAttacking)
        {
            return;
        }


        Entity entity =
            other.GetComponent<Entity>();

        if (entity == null)
        {
            return;
        }


        // 設置者にはダメージを与えない
        if (entity == m_owner)
        {
            return;
        }


        // TrapBaseのDamageDataを使用
        entity.TakeDamage(m_damageData);

        OnHit();
    }
}