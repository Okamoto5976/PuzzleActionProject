using UnityEngine;


public struct DamageData
{
    public int m_damage;
    public DamageType m_type;

    public float m_knockbackForce;
    public Vector3 m_hitPoint;

    public GameObject m_hitEffect;
    public AudioClip m_hitSound;
}


public enum DamageType//ダメージタイプ
{
    Normal
}

public abstract class EntityHP:MonoBehaviour 
{
    [Header("HP設定")] 
    public int m_maxHP = 100;
    public int m_currentHP;

    protected Rigidbody rb;
    protected virtual void Start() 
    {
        m_currentHP = m_maxHP;
        rb = GetComponent<Rigidbody>();
    }
    //ダメージを受ける
    public virtual void TakeDamage(DamageData data)
    {
        m_currentHP -= data.m_damage;
        Debug.Log("ダメージ :" + data.m_damage + " 残りHP :" + m_currentHP);

        //ノックバック
        if(rb!=null)
        {
            Vector3 dir = (transform.position - data.m_hitPoint).normalized;
            rb.AddForce(dir * data.m_knockbackForce, ForceMode.Impulse);
        }
        //エフェクト (Managerに任せる）

        if (data.m_hitEffect!=null)
        {
            //Instantiate(data.hitEffect, data.hitPoint, Quaternion.identity);
        }
        //サウンド
        if(data.m_hitSound!=null)
        {
           //AudioSource.PlayClipAtPoint(data.hitSound, transform.position);
        }
        //死亡処理
        if (m_currentHP <= 0)
        {
            Die();
        }
    }
    //ここがabstract
    protected abstract void Die();
}
