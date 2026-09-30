using UnityEngine;

abstract public class EntityHP : MonoBehaviour
{
    protected Entity m_entity;

    private AudioSource m_audioSource;

    [SerializeField] private int m_currentHP;
    [SerializeField] private int m_max;
    public int CurrentHP { get => m_currentHP;}

    public int MaxHP => (int)m_entity.HP;

    

    [SerializeField] private DamageParticleController m_damageParticleController;

    private void Awake()
    {
        m_entity = GetComponent<Entity>();

        m_audioSource=GetComponent<AudioSource>();
    }

    private void Update()
    {
        m_max = MaxHP;
    }

    private void Start()
    {
        if (m_entity == null) return;
        m_currentHP = (int)m_entity.HP;
    }

    public virtual void TakeDamage(DamageData data)//DamageData
    {

        //float hitRate =
        //    data.HitRate - m_entity.DEX;

        //hitRate = Mathf.Clamp(hitRate, 0, 100);

        //if(Random.Range(0f,100f)>hitRate)
        //{
        //    Debug.Log("Miss");
        //    return;
        //}

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

        float damage = 0;

        //Break
        if(isBreak)
        {
            damage = 9999;

            Debug.Log($"{gameObject.name}のダメージ処理 : {damage} = BreakAttack");

        }
        else
        {
            //クリティカルを先にアタックにかけて　
            if (isCritical)
            {
                damage = (data.Attack * data.CriticalDamage);
            }
            else
            {
                damage = data.Attack;
            }

              damage = Mathf.Max(damage - (int)m_entity.DEF, 0);

            if(isCritical)
            {
                Debug.Log($"{gameObject.name}のダメージ処理 : {damage} = ( Attack : {data.Attack} * CD : {data.CriticalDamage}) - DEF : {m_entity.DEF}");

            }
            else
            {
                Debug.Log($"{gameObject.name}のダメージ処理 : {damage} = Attack : {data.Attack} - DEF : {m_entity.DEF}");

            }
        }

            m_currentHP -= (int)damage;



        if(isCritical)
        {
            if (m_damageParticleController != null)
            {
                m_damageParticleController.DoDamageParticle((uint)damage, DamageParticleType.Critical);
            }
        }
        else if(isBreak)
        {
            if (m_damageParticleController != null)
            {
                m_damageParticleController.DoDamageParticle((uint)damage, DamageParticleType.Break);
            }
        }
        else
        {
            if (m_damageParticleController != null)
            {
                m_damageParticleController.DoDamageParticle((uint)damage, DamageParticleType.Normal);
            }
        }

        m_currentHP = Mathf.Max(m_currentHP, 0);


        //Debug.Log($"{gameObject.name} : {damage}damage");

        //Debug.Log($"{gameObject.name} HP : {m_currentHP}");

        //if(m_entity.DamageSE !=null&&m_audioSource!=null)
        //{
        //    m_audioSource.PlayOneShot(m_entity.DamageSE);
        //}

        float knockBackPower = Mathf.Clamp(data.Knockback - m_entity.DEF, 0f, 3f);
        Vector3 dir = data.AttackDir.normalized;


        m_entity.ApplyKnockBack(dir, knockBackPower);


        m_entity.ApplyStun(data.StunDuration);


        if ( m_currentHP <= 0 ) 
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (m_damageParticleController != null)
        {
            m_damageParticleController.DoDamageParticle((uint)amount, DamageParticleType.Heal);
        }

        m_currentHP = Mathf.Min(m_currentHP + Mathf.FloorToInt(amount), (int)m_entity.HP);
    }

    public void TakeBuffDamage(StatusType type, float damage)
    {
        //damage color
        switch(type)
        {
            case StatusType.Gas:
                if (m_damageParticleController != null)
                {
                    Debug.Log("damage particle");
                    m_damageParticleController.DoDamageParticle((uint)damage, DamageParticleType.Gas);
                }
                break;
            case StatusType.Poison:
                if (m_damageParticleController != null)
                {
                    Debug.Log("damage particle");
                    m_damageParticleController.DoDamageParticle((uint)damage, DamageParticleType.Poison);
                }
                break;
            case StatusType.Burn:
                if (m_damageParticleController != null)
                {
                    Debug.Log("damage particle");
                    m_damageParticleController.DoDamageParticle((uint)damage, DamageParticleType.Burn);
                }
                break;
        }

        m_currentHP = Mathf.Max(1, m_currentHP - Mathf.FloorToInt(damage));
    }

    public void ResetHP()
    {
        if(m_entity == null)
        {
            m_entity = GetComponent<Entity>();
        }
        m_currentHP = (int)m_entity.HP;
    }

    protected abstract void Die();
       
}
