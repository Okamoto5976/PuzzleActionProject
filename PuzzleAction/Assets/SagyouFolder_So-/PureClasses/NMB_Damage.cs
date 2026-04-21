using UnityEngine;

[System.Serializable]
public class NMB_Damage : NMB_MBAdapter<NMB_Damage>
{
    [Header("Damage")]
    [SerializeField] private float m_hp;

    public float HP => m_hp;


    public void DealDamage(float damage)
    {
        m_hp -= damage;
        if (m_hp < 0)
        {
            Die();
        }
    }

    public void Die()
    {

    }
}
