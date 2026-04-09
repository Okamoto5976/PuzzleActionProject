using UnityEngine;


public struct DamageData
{
    public int damage;
    public DamageType type;

    public float knockbackForce;
    public Vector3 hitPoint;

    public GameObject hitEffect;
    public AudioClip hitSound;
}


public enum DamageType//ダメージタイプ
{
    Normal
}

public abstract class EntityHP:MonoBehaviour,IDamage 
{
    [Header("HP設定")] 
    public int maxHP = 100;
    public int currentHP;

    protected Rigidbody rb;
    protected virtual void Start() 
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody>();
    }
    //ダメージを受ける
    public virtual void TakeDamage(DamageData data)
    {
        currentHP -= data.damage;
        Debug.Log("ダメージ :" + data.damage + " 残りHP :" + currentHP);

        //ノックバック
        if(rb!=null)
        {
            Vector3 dir = (transform.position - data.hitPoint).normalized;
            rb.AddForce(dir * data.knockbackForce, ForceMode.Impulse);
        }
        //エフェクト (Managerに任せる）

        if (data.hitEffect!=null)
        {
            //Instantiate(data.hitEffect, data.hitPoint, Quaternion.identity);
        }
        //サウンド
        if(data.hitSound!=null)
        {
           //AudioSource.PlayClipAtPoint(data.hitSound, transform.position);
        }
        //死亡処理
        if (currentHP <= 0)
        {
            Die();
        }
    }
    //ここがabstract
    protected abstract void Die();
}
