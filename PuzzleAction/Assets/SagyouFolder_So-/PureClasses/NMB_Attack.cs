using UnityEngine;

[System.Serializable]
public class NMB_Attack : NMB_MBAdapter<NMB_Attack>
{
    [Header("Attack")]
    [SerializeField] private float m_damage;
    [SerializeField] private Collider m_damageArea;


    public void AttackEntity(MB_Entity target)
    {
        target.MyEnemyInfo.damage.DealDamage(m_damage);
    }
}
