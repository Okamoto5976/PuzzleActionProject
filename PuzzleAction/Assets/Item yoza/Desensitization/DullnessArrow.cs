using UnityEngine;

[RequireComponent(typeof(ReturnObjectToPool))]
public class DullnessPoisnon : TrapBase
{
    [Header("デバフ設定")]
    [SerializeField] private float m_duration = 5f; // 持続時間
    [SerializeField] private BuffID m_buffID;       // 毒用のBuffID

    [Header("低下率パラメーター")]
    [SerializeField] private float m_critRateReduction = 0.2f; // クリティカル率低下量

    protected override void SetUp()
    {
    
    }
    
    protected override void OnHit()
    {
    
    }
    /// <summary>
    /// setti→2byou→yuukouka→10byoutennkai→poolhennkyaku
    /// </summary>

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Entity>() is { } victim)
        {
            // 自分のチームには当たらないように判定
            if (m_owner != null && victim.Team == m_owner.Team) return;
            if (victim.IsStun || victim.CurrentState == Entity.EntityState.Dead) return;

            // 1. クリティカル率低下デバフを付与
            if (victim.TryGetComponent<EntityTemporaryBuffSystem>(out var buffSystem))
            {
                StatusModifier critModifier = new StatusModifier();
                critModifier.m_statType = StatusType.CriticalRate;
                critModifier.m_value = -m_critRateReduction;

                float critical = victim.CriticalRate;

                buffSystem.AddBuff(critModifier, m_buffID, m_duration);

                float Debuffcritical = victim.CriticalRate;

                Debug.Log($"[毒効果確認] {victim.name} | 変化前: {critical} -> 変化後: {Debuffcritical}");
            }

            // 2. ダメージ適用処理（DamageData が必要な場合）
            if (m_damageData != null)
            {
                victim.TakeDamage(m_damageData);
            }

            Debug.Log($"{victim.name} に鈍化矢が命中（クリティカル率低下）");

            // 3. 命中したらプールへ返却
            OnReturnPool();
        }
    }
}
