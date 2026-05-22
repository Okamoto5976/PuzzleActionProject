using UnityEngine;

abstract public class EntityHP : MonoBehaviour
{
    private Entity m_entity;

    private int m_currentHP;
    //HPBerなどに
    public int CurrentHP { get => m_currentHP;}

    private void Awake()
    {
        m_entity = GetComponent<Entity>();


    }

    private void Start()
    {
        if (m_entity == null) return;
        m_currentHP = (int)m_entity.HP;
    }

    public void TakeDamage(int damage )
    {
        m_currentHP -= damage;

        m_currentHP = Mathf.Max( m_currentHP, 0 );

        Debug.Log($"{gameObject.name} : {damage}damage");

        Debug.Log($"現在HP : {m_currentHP}");

        //ダメージ計算

        //命中率基本１
        //DEX確率
        //BreakRate率でtrueなら9999
        //Criticalかどうかbool
        //HP -= (STR - DEF) * critical
        //ノックバック（stunResで軽減）大きさで復帰の速度変更
        //スタン確率 10% 1秒　　100％　10秒


        //effect audio再生 hit時の
        //攻撃の音は　与ダメのほうが
        //攻撃された音は被ダメのほうが
        //被ダメ側は自身の特徴にある被ダメ音を持つ 例）スライム　は　粘着度のある音
        //特殊な音がある場合は　ダメージDataに渡す　（炎の音とか



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
