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
        m_Center = pos;
        m_Radius = radius;
        m_DamageAmount = damage;

        transform.position = pos;
    }
    void Update()
    {
        m_Timer += Time.deltaTime;

        //シーン内すべてのトラップ対象を検索
        karitesuto[] allTargets = Object.FindObjectsByType<karitesuto>(FindObjectsSortMode.None);

        foreach (var target in allTargets)
        {
            if (target == null) continue;
            //距離計算
            float distance = Vector3.Distance(m_Center, target.transform.position);
            //範囲内にいるとき
            if (distance <= m_Radius)
            {
                //Nature以外
                if (target.m_MyTeam != TrapTeam.Nature)
                {
                    if (m_Timer >= m_DamageInterval)
                    {
                        //仮ログ
                        Debug.Log($"[GAS]{target.gameObject.name}({target.m_MyTeam}に{m_DamageAmount}ダメージ!)");

                        //一応
                        // target.GetComponent<HPスクリプト>().ReduceHP(m_DamageAmount);
                    }
                }
            }
        }
        //タイマーループ
        if (m_Timer >= m_DamageInterval)
        {
            m_Timer = 0f;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_Radius);
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, m_Radius);
    //}

}
