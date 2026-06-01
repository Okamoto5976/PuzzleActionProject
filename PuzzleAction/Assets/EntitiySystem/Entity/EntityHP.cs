using UnityEngine;

abstract public class EntityHP : MonoBehaviour
{
    private Entity m_entity;

    private AudioSource m_audioSource;

    private int m_currentHP;
    //HPBer�Ȃǂ�
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

    //trap���ŏ����������ł���悤virtual��
    //trap��HP�̂Ȃ����̂����邽��
    public virtual void TakeDamage(Entity attacker)//DamageData
    {
        //�U�����Đ�
        if (attacker.AttackSE != null)
        {
            if (attacker.AudioSource != null)
            {
                attacker.AudioSource.PlayOneShot(attacker.AttackSE);
            }
        }

        //������
        float hitRate =
            100f + attacker.DEX - m_entity.DEX;

        hitRate = Mathf.Clamp(hitRate, 0, 100);

        if(Random.Range(0f,100f)>hitRate)
        {
            Debug.Log("Miss");
            return;
        }

        //Break����
        bool isBreak = false;

        if(Random.Range(0f,100f)<=attacker.BreakRate)
        {
            isBreak = true;
        }

        //Critical����
        bool isCritical = false;

        if(Random.Range(0f,100f)<=attacker.CriticalRate)
        {
            isCritical = true;
        }

        //�_���[�W�v�Z
        int damage = 0;

        //Break
        if(isBreak)
        {
            damage = 9999;
        }
        else
        {
            //��{�_���[�W
            damage = Mathf.Max((int)(attacker.STR - m_entity.DEF), 1);

            //Critical
            if(isCritical)
            {
                damage = (int)(damage * attacker.CriticalDamage);
            }
        }

        //HP����
            m_currentHP -= damage;

        m_currentHP = Mathf.Max( m_currentHP, 0 );

        Debug.Log($"{gameObject.name} : {damage}damage");

        Debug.Log($"����HP : {m_currentHP}");

        //��_�����Đ�
        if(m_entity.DamageSE !=null&&m_audioSource!=null)
        {
            m_audioSource.PlayOneShot(m_entity.DamageSE);
        }

        //�m�b�N�o�b�N
        float knockBackPower = Mathf.Max(attacker.KnockBack - m_entity.DEF, 0);

        //�X�^��
        float stunPower = Mathf.Max(attacker.Stun - m_entity.StunRes, 0);

        float stunTime=stunPower * 0.1f;

        //����
        //���@�ʂ̂Ƃ��납��擾
        Vector3 dir =(transform.position-attacker.transform.position).normalized;

        //������΂�
        m_entity.ApplyKnockBack(dir, knockBackPower,stunTime);

        //SE
        //��_��SE

        //�_���[�W�v�Z

        //��������{100
        //DEX�m��
        //BreakRate����true�Ȃ�9999
        //Critical���ǂ���bool
        //HP -= (STR - DEF) * critical
        //�m�b�N�o�b�N�istunRes�Ōy���j�傫���ŕ��A�̑��x�ύX
        //�X�^���m�� 10% 1�b�@�@100���@10�b

        //Damage();
        //KnockBack();

        //effect audio�Đ� hit����
        //�U���̉��́@�^�_���̂ق���
        //�U�����ꂽ���͔�_���̂ق���
        //��_�����͎��g�̓����ɂ����_���������� ��j�X���C���@�́@�S���x�̂��鉹
        //����ȉ�������ꍇ�́@�_���[�WData�ɓn���@�i���̉��Ƃ�

        //���S

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
