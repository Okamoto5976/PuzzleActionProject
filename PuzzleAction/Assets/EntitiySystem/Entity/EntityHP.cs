using UnityEngine;

abstract public class EntityHP : MonoBehaviour
{
    private Entity m_entity;

    private AudioSource m_audioSource;

    private int m_currentHP;
    public int CurrentHP { get => m_currentHP;}

    private void Awake()
    {
        m_entity = GetComponent<Entity>();

        m_audioSource=GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (m_entity == null) return;
        m_currentHP = (int)m_entity.HP;
    }

    public virtual void TakeDamage(DamageData data)//DamageData
    {
        switch (data.AttackType)
        {
            case AttackType.None:
                break;

            case AttackType.Recovery:
                Heal(data.Attack);
                break;

            case AttackType.Fire:
                break;
        }

        if (data.AttackerSE != null)
        {
            if (data.AudioSource != null)
            {
                data.AudioSource.PlayOneShot(data.AttackerSE);
            }
        }

        float hitRate =
            data.HitRate - m_entity.DEX;

        hitRate = Mathf.Clamp(hitRate, 0, 100);

        if(Random.Range(0f,100f)>hitRate)
        {
            Debug.Log("Miss");
            return;
        }

        bool isBreak = false;

        if(Random.Range(0f,100f)<=data.BreakRate)
        {
            isBreak = true;
        }

        bool isCritical = false;

        if(Random.Range(0f,100f)<=data.CriticalRate)
        {
            isCritical = true;
        }

        int damage = 0;

        //Break
        if(isBreak)
        {
            damage = 9999;
        }
        else
        {
            damage = Mathf.Max(data.Attack -(int) m_entity.DEF, 1);

            //Critical
            if(isCritical)
            {
                damage = (int)(damage * data.CriticalDamage);
            }
        }

            m_currentHP -= damage;

        m_currentHP = Mathf.Max( m_currentHP, 0 );

        Debug.Log($"{gameObject.name} : {damage}damage");

        Debug.Log($"HP : {m_currentHP}");

        if(m_entity.DamageSE !=null&&m_audioSource!=null)
        {
            m_audioSource.PlayOneShot(m_entity.DamageSE);
        }

        float knockBackPower = Mathf.Max(data.Knockback - m_entity.DEF, 0);

        float stunPower = Mathf.Max(data.Stun - m_entity.StunRes, 0);

        float stunTime=stunPower * 0.1f;

        Vector3 dir = data.AttackDir.normalized;

        m_entity.ApplyKnockBack(dir, knockBackPower,stunTime);

        if ( m_currentHP <= 0 ) 
        {
            Die();
        }
    }

    public void Damage(int value)
    {

    }

    public void KnockBack(int value)
    {
        //m_entity.KnockBack(value)
    }



    public void Heal(int amount)
    {
        m_currentHP = Mathf.Min(m_currentHP + amount, (int)m_entity.HP);
    }

    protected abstract void Die();
       
}
