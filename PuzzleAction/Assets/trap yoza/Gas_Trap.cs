using UnityEngine;


public class Gas_Trap : Entity
{
    private Vector3 m_Center;
    private float m_Radius;
    private int m_DamageAmount;

    private float m_DamageInterval = 1.0f;
    private float m_Timer;
   
    ///<summary>
    ///アイテム側から呼ぶ初期化
    /// </summary>
    public void Init(Vector3 pos, float radius, int damage)
    {
        m_DamageAmount = damage;
        transform.position = pos;
    }
    void Update()
    {
        m_Timer += Time.deltaTime;
    }
    private void OnTriggerStay(Collider other)
    {
        // タイマーのタイミングがちょうど来たらダメージを計算する
        if (m_Timer >= m_DamageInterval)
        {
            Entity target = other.GetComponent<Entity>();
            if (target != null)
            {
                // Nature（自然・中立）チーム以外ならダメージ！
                if (target.Team == Entity.Teamtype.Nature)
                {
                    m_Timer = 0;
                    Debug.Log($"[GAS_BOX] {target.gameObject.name} ({target.Team}) に四角いエリア内で {m_DamageAmount} ダメージ！");

                    // target.GetComponent<HPスクリプト>().ReduceHP(m_DamageAmount);
                }
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        BoxCollider box=GetComponent<BoxCollider>();
        if (box != null) 
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + box.center, box.size);
        }
    }
}
