using UnityEngine;

public class EntityHP:MonoBehaviour,EntityHP.IDamage 
{
    [Header("HP設定")] 
    public int maxHP = 100;
    public int currentHP;

   public enum DamageType//ダメージタイプ
    {
        Normal
    }

    public interface IDamage//インターフェース
    {
        void TakeDamage(DamageData data);
    }
    [System.Serializable]
    public struct DamageData
    {
        public int damage;
        public DamageType type;

        public float knockbackForce;
        public Vector3 hitPoint;

        public GameObject hitEffect;
        public AudioClip hitSound;
    }
    Rigidbody rb;
    private void Start() 
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody>();
    }
    //ダメージを受ける
    public void TakeDamage(DamageData data)
    {
        currentHP -= data.damage;
        Debug.Log("ダメージ :" + data.damage + " 残りHP :" + currentHP);

        //ノックバック
        if(rb!=null)
        {
            Vector3 dir = (transform.position - data.hitPoint).normalized;
            rb.AddForce(dir * data.knockbackForce, ForceMode.Impulse);
        }
        //エフェクト
        if(data.hitEffect!=null)
        {
            Instantiate(data.hitEffect, data.hitPoint, Quaternion.identity);
        }
        //サウンド
        if(data.hitSound!=null)
        {
            AudioSource.PlayClipAtPoint(data.hitSound, transform.position);
        }
        //死亡処理
        if (currentHP <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("死亡");
        //Destroy(gameObject);
    }
}
