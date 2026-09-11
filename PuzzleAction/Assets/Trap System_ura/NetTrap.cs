using UnityEngine;

public class NetTrap : TrapBase
{
    [Header("Stun")]
    [SerializeField] private float m_stunTime = 2.0f;

    [Header("Detection")]
    [SerializeField] private float m_range = 5.0f;
    [SerializeField] private float m_angle = 60.0f;
    [SerializeField] private LayerMask m_targetLayer;

    protected override void SetUp()
    {

    }

    public void Activate()
    {
        // 周囲のColliderを取得
        Collider[] targets = Physics.OverlapSphere(
            transform.position,
            m_range,
            m_targetLayer
        );

        foreach (Collider target in targets)
        {
            Entity entity = target.GetComponentInParent<Entity>();

            if (entity == null)
                continue;

            // 自分自身には当てない
            if (entity == m_owner)
                continue;

            // 自分から敵への方向
            Vector3 dir =
                entity.transform.position - transform.position;

            dir.y = 0.0f;

            // 前方にいるか確認
            float angle =
                Vector3.Angle(transform.forward, dir.normalized);

            if (angle > m_angle / 2.0f)
                continue;

            // スタン効果
            Stun(entity);
        }
    }

    private void Stun(Entity target)
    {
        Debug.Log(target.name + " が " + m_stunTime + "秒スタン！");
    }

    protected override void OnHit()
    {
        Activate();
    }
}