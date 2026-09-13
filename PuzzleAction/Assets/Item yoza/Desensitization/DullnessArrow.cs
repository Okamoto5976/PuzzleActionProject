using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DullnessPoisnon : TrapBase
{
    [Header("デバフ設定")]
    [SerializeField] private float m_duration = 5f; // 持続時間
    //[SerializeField] private BuffID m_buffID;       // 毒用のBuffID

    [Header("低下率パラメーター")]
    [SerializeField] private float m_critRateReduction = 0.2f; // クリティカル率低下量

    //[SerializeField] private float m_power;

    [SerializeField] private LayerMask m_hitLayers;

    private bool m_isInitialized;

    private void FixedUpdate()
    {
        if (!m_isInitialized)
            return;

        OnAddForce(m_dir, m_power);

        m_isInitialized = false;
    }

    protected override void EntitySetUp()
    {
        m_rb.linearVelocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;
        m_isInitialized = true;
    }
    
    protected override void OnHit()
    {
        OnReturnPool();

    }
    
    private StatusModifier SetModifier()
    {
        StatusModifier modifier = new StatusModifier()
        {
            m_statType = StatusType.CriticalRate,
            m_value = 20f,
            m_modType = ModifierType.Subtract,
        };

        return modifier;
    }

    /// <summary>
    /// setti→2byou→yuukouka→10byoutennkai→poolhennkyaku
    /// </summary>
    /// 
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Entity>() is { } victim)
        {
            if ((m_hitLayers.value & (1 << victim.gameObject.layer)) != 0)
            {
                OnHit();
                return;
            }

            // 自分のチームには当たらないように判定
            //if (m_owner != null && victim.Team == m_owner.Team) return;
            if (victim.Team == m_team) return;

            //if (victim.IsStun || victim.CurrentState == Entity.EntityState.Dead) return;

            // 1. クリティカル率低下デバフを付与
            //if (victim.TryGetComponent<EntityTemporaryBuffSystem>(out var buffSystem))
            //{
            //    StatusModifier critModifier = new StatusModifier();
            //    critModifier.m_statType = StatusType.CriticalRate;
            //    critModifier.m_value = -m_critRateReduction;

            //    float critical = victim.CriticalRate;

            //    buffSystem.AddBuff(critModifier, BuffID.CriticalRate, m_duration);

            //    float Debuffcritical = victim.CriticalRate;

            //    Debug.Log($"[毒効果確認] {victim.name} | 変化前: {critical} -> 変化後: {Debuffcritical}");
            //}

            var modifier = SetModifier();

            victim.AddBuff(modifier, BuffID.CriticalRate, m_duration);

            // 2. ダメージ適用処理（DamageData が必要な場合）
            if (m_damageData != null)
            {
                victim.TakeDamage(m_damageData);
            }

            //Debug.Log($"{victim.name} に鈍化矢が命中（クリティカル率低下）");

            // 3. 命中したらプールへ返却
            OnHit();
        }
    }
}
