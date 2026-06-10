using System.Collections.Generic;
using UnityEngine;

public class Swamp_Trap : Entity
{
    private Vector3 m_Center;
    private float m_Radius;
    private float m_Slow;

   
    // 現在沼の中で減速しているオブジェクト記録
    private List<Entity> m_SlowedTargets = new List<Entity>();

    ///<summary>
    ///アイテム側から呼ぶ初期化
    /// </summary>
    public void Init(Vector3 pos,float radius,float slowMultiplier)
    {
        m_Slow = slowMultiplier;
        transform.position = pos;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Entity target = other.GetComponent<Entity>();
        if (target != null && target.Team ==TeamType.Nature)
        {
            if (!m_SlowedTargets.Contains(target))
            {
                m_SlowedTargets.Add(target);
                Debug.Log($"[SWAMP_BOX] {target.gameObject.name} が四角い沼に入った！速度を -{m_Slow} 倍にします");
            }
        }
    }

   
    private void OnTriggerExit(Collider other)
    {
        Entity target = other.GetComponent<Entity>();
        if (target != null && m_SlowedTargets.Contains(target))
        {
            Debug.Log($"[SWAMP_BOX] {target.gameObject.name} が四角い沼から出た！速度を戻します");
            m_SlowedTargets.Remove(target);
        }
    }

    private void OnDestroy()
    {
        // 罠が途中で消えるときに、減速していた敵の速度をちゃんと安全に戻してあげる処理
        foreach (var target in m_SlowedTargets)
        {
            if (target == null)
            {
                return;
                // 速度を戻す処理
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        if (box != null)
        {
            Gizmos.color = new Color(0.5f, 0.25f, 0f); // 茶色っぽい線
            Gizmos.DrawWireCube(transform.position + box.center, box.size);
        }
    }
}  

