using System.Collections.Generic;
using UnityEngine;

public class Swamp_Trap : MonoBehaviour
{
    private Vector3 m_Center;
    private float m_Radius;
    private float m_Slow;

   
    // 現在沼の中で減速しているオブジェクト記録
    private List<karitesuto> m_SlowedTargets = new List<karitesuto>();

    ///<summary>
    ///アイテム側から呼ぶ初期化
    /// </summary>
    public void Init(Vector3 pos,float radius,float slowMultiplier)
    {
        m_Center = pos;
        m_Radius = radius;
        m_Slow = slowMultiplier;

        transform.position = pos;
    }
    void Update()
    {
        karitesuto[] allTargets = Object.FindObjectsByType<karitesuto>(FindObjectsSortMode.None);
        List<karitesuto> currentInRadius = new List<karitesuto>();
        foreach (var target in allTargets)
        {
            if (target == null) continue;
            //距離計算
            float distance = Vector3.Distance(m_Center, target.transform.position);

            if (distance <= m_Radius)
            {
                if (target.m_MyTeam != TrapTeam.Nature)
                {
                    currentInRadius.Add(target);
                    //沼地に入った場合
                    if (!m_SlowedTargets.Contains(target))
                    {
                        m_SlowedTargets.Add(target);
                        Debug.Log($"[SWAMP]{target.gameObject.name}({target.m_MyTeam})が沼に入った！速度を-{m_Slow}倍にします");
                    }
                }
            }
        }
        //範囲内から出たオブジェクトの速度を戻す
        for(int i=m_SlowedTargets.Count-1; i>=0; i--)
        {
            karitesuto target=m_SlowedTargets[i];
            if (target == null || !currentInRadius.Contains(target))
            {
                if (target != null)
                {
                    Debug.Log($"[SWAMP]{target.gameObject.name}({target.m_MyTeam})が沼から出た！速度戻す");
                　　//速度落戻す処理
                }
                m_SlowedTargets.RemoveAt(i);
            }
        }
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.5f, 0.25f, 0f);
        Gizmos.DrawWireSphere(transform.position, m_Radius);
    }

   // private void OnDrawGizmos()
   // {
   //     Gizmos.color = new Color(0.5f, 0.25f, 0f);
   //     Gizmos.DrawWireSphere(transform.position, m_Radius);
   // }
}  

