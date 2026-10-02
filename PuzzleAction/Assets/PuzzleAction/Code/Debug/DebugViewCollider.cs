using UnityEngine;
using System.Collections;

public class DebugViewCollider : MonoBehaviour
{
    public static DebugViewCollider Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private AttackHitBox m_currentHitBox;
    private AttackRay m_currentRay;

    private bool m_isHitVisible;

    private bool m_isRayVisible;

    private Coroutine m_viewCoroutine;

    public void ViewHitCollider(AttackHitBox box)
    {
        m_currentHitBox = box;

        if (m_viewCoroutine != null) return;

        m_viewCoroutine = StartCoroutine(ViewHitColliderTime());
    }

    public void ViewRayCollider(AttackRay ray)
    {
        m_currentRay = ray;

        if (m_viewCoroutine != null) return;

        m_viewCoroutine = StartCoroutine(ViewRayColliderTime());
    }

    private IEnumerator ViewHitColliderTime()
    {
        m_isHitVisible = true;
        yield return new WaitForSeconds(0.5f);
        m_isHitVisible = false;

        m_viewCoroutine = null;

        yield break;
    }

    private IEnumerator ViewRayColliderTime()
    {
        m_isRayVisible = true;
        yield return new WaitForSeconds(0.5f);
        m_isRayVisible = false;

        m_viewCoroutine = null;

        yield break;
    }

    private void OnDrawGizmos()
    {
        if (m_isHitVisible)
        {
            if (m_currentHitBox == null) return;
            //Debug.Log("DrawGizmos");

            Gizmos.color = Color.red;

            Vector3 center = m_currentHitBox.m_pos + m_currentHitBox.m_hitBoxOffset;
            Gizmos.DrawWireSphere(center, m_currentHitBox.m_radius);
        }
        else if(m_isRayVisible)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawRay(
                m_currentRay.origin,
                m_currentRay.direction * m_currentRay.range
            );
        }
    }

}
