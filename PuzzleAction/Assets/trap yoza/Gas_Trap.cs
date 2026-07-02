using UnityEngine;


public class Gas_Trap : MonoBehaviour
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
        m_Timer = 0f;
    }
    void Update()
    {
        m_Timer += Time.deltaTime;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!enabled) return;
        // タイマーのタイミングがちょうど来たらダメージを計算する
        if (m_Timer >= m_DamageInterval)
        {
            karitesuto target = other.GetComponent<karitesuto>();
            if (target != null && target.m_MyTeam != TrapTeam.Nature)
            {
                m_Timer = 0;
                Debug.Log($"[GAS_BOX] {target.gameObject.name} ({target.m_MyTeam}) に四角いエリア内で {m_DamageAmount} ダメージ！");

                // target.GetComponent<HPスクリプト>().ReduceHP(m_DamageAmount);

            }
        }
    }
    public void ReleaseToPool()
    {
        m_Timer = 0f;
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        if (!enabled) return;
        BoxCollider box=GetComponent<BoxCollider>();
        if (box != null) 
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + box.center, box.size);
        }
    }
}
