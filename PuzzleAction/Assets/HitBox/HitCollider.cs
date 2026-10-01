using System.Collections;
using UnityEngine;

[System.Serializable]
public class AttackHitBox
{
    public Transform m_transform;
    public Vector3 m_hitBoxOffset;
    public float m_radius;
}

public class HitCollider
{
    public HitCollider(bool isVisible)
    {
        m_isVisible = isVisible;
    }

    private bool m_isViewCollider;

    private bool m_isVisible;

    
    private AttackHitBox m_currentHitBox;

    protected Coroutine m_viewCoroutine;
    
    public void AttackCollider(DamageData data, TeamType myTeam, AttackHitBox attackHitBox)
    {
        // ヒットした判定のセット
        //HashSet<Entity> hitSet = new();
        {
            m_currentHitBox = attackHitBox;

            Collider[] hits = Physics.OverlapSphere(
                attackHitBox.m_transform.position + attackHitBox.m_hitBoxOffset,
                attackHitBox.m_radius
                       );


            //Debug.Log($"hits.Length : {hits.Length}");

            foreach (var hit in hits)
            {
                Entity entity = hit.GetComponentInParent<Entity>();

                if (entity == null)
                {
                    continue;
                }
                if(entity.Team==myTeam)
                {
                    continue;
                }

                entity.TakeDamage(data);

                
            }

        }

        if (m_isViewCollider)
        {
            if (m_viewCoroutine != null) return;

            OnDrawGizmos();
            //m_viewCoroutine = StartCoroutine(ViewColliderTime());
        }

    }

    private IEnumerator ViewColliderTime()
    {
        m_isVisible = true;
        yield return new WaitForSeconds(0.5f);
        m_isVisible = false;

        m_viewCoroutine = null;

        yield break;
    }

    private void OnDrawGizmos()
    {
        if (!m_isVisible) return;
        if (m_currentHitBox == null) return;
        //Debug.Log("DrawGizmos");

        Gizmos.color = Color.red;

        Vector3 center = m_currentHitBox.m_transform.position + m_currentHitBox.m_hitBoxOffset;
        Gizmos.DrawWireSphere(center, m_currentHitBox.m_radius);
    }
}
