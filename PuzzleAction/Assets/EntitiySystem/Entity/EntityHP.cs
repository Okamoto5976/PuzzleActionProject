using UnityEngine;

abstract public class EntityHP : MonoBehaviour
{
    private Entity m_entity;

    private AudioSource m_audioSource;

    private int m_currentHP;
    //HPBerなどに
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

    //trap側で書き換えができるようvirtualに
    //trapはHPのないものもあるため
    public virtual void TakeDamage(Entity attacker)//DamageData
    {
        //攻撃音再生
        if (attacker.AttackSE != null)
        {
            if (attacker.AudioSource != null)
            {
                attacker.AudioSource.PlayOneShot(attacker.AttackSE);
            }
        }

        //命中率
        float hitRate =
            100f + attacker.DEX - m_entity.DEX;

        hitRate = Mathf.Clamp(hitRate, 0, 100);

        if(Random.Range(0f,100f)>hitRate)
        {
            Debug.Log("Miss");
            return;
        }

        //Break判定
        bool isBreak = false;

        if(Random.Range(0f,100f)<=attacker.BreakRate)
        {
            isBreak = true;
        }

        //Critical判定
        bool isCritical = false;

        if(Random.Range(0f,100f)<=attacker.CriticalRate)
        {
            isCritical = true;
        }

        //ダメージ計算
        int damage = 0;

        //Break
        if(isBreak)
        {
            damage = 9999;
        }
        else
        {
            //基本ダメージ
            damage = Mathf.Max((int)(attacker.STR - m_entity.DEF), 1);

            //Critical
            if(isCritical)
            {
                damage = (int)(damage * attacker.CriticalDamage);
            }
        }

        //HP減少
            m_currentHP -= damage;

        m_currentHP = Mathf.Max( m_currentHP, 0 );

        Debug.Log($"{gameObject.name} : {damage}damage");

        Debug.Log($"現在HP : {m_currentHP}");

        //被ダメ音再生
        if(m_entity.DamageSE !=null&&m_audioSource!=null)
        {
            m_audioSource.PlayOneShot(m_entity.DamageSE);
        }

        //ノックバック
        float knockBackPower = Mathf.Max(attacker.KnockBack - m_entity.DEF, 0);

        //スタン
        float stunPower = Mathf.Max(attacker.Stun - m_entity.StunRes, 0);

        float stunTime=stunPower * 0.1f;

        //方向
        //仮　別のところから取得
        Vector3 dir =(transform.position-attacker.transform.position).normalized;

        //吹っ飛ばし
        m_entity.ApplyKnockBack(dir, knockBackPower,stunTime);

        //SE
        //被ダメSE

        //ダメージ計算

        //命中率基本100
        //DEX確率
        //BreakRate率でtrueなら9999
        //Criticalかどうかbool
        //HP -= (STR - DEF) * critical
        //ノックバック（stunResで軽減）大きさで復帰の速度変更
        //スタン確率 10% 1秒　　100％　10秒

        //Damage();
        //KnockBack();

        //effect audio再生 hit時の
        //攻撃の音は　与ダメのほうが
        //攻撃された音は被ダメのほうが
        //被ダメ側は自身の特徴にある被ダメ音を持つ 例）スライム　は　粘着度のある音
        //特殊な音がある場合は　ダメージDataに渡す　（炎の音とか

        //死亡

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
