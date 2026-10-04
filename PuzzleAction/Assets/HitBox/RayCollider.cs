using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackRay
{
    public Vector3 origin;
    public Vector3 direction;
    public float range;  //射程距離
    public int maxPenetrate;    // 最大貫通数
}

// 壁の貫通対策は未実装
public class RayCollider
{
    public RayCollider(bool IsVisible)
    {
        m_isVisible = IsVisible;
    }

    private Ray m_ray;
    private bool m_isVisible;  // UnityEditorでHitBoxの当たり判定を表示するか


    public List<Collider> AttackCollider(DamageData data, TeamType myTeam, AttackRay attackRay)
    {
        // ヒットした判定のセット
        List<Collider> hitSet = new();

        m_ray = new Ray(attackRay.origin, attackRay.direction);

        if (m_isVisible)
        {
            Debug.DrawRay(attackRay.origin, attackRay.direction * attackRay.range, Color.red, 2.0f);
        }

        RaycastHit hit;
        //Debug.Log($"penetrate: {attackRay.maxPenetrate}");

        for (int i = 0; i < attackRay.maxPenetrate; i++)
        {
            if (Physics.Raycast(m_ray, out hit, attackRay.range))
            {
                //Debug.Log($"hit.name: {hit.collider.name}");

                hitSet.Add(hit.collider);
                //hitSet[i].enabled = false;  // 2回以上は判定しないようにする
            }
            else
            {
                break;
            }
        }

        // 判定を戻す

        return hitSet;
        
        //foreach (var col in hitSet)
        //{
        //    col.enabled = true;

        //    Entity entity = col.GetComponentInParent<Entity>();
        //    if (entity == null) continue;
        //    if (entity.Team == myTeam) continue;

        //    entity.TakeDamage(data);
        //}

        

    }
}
