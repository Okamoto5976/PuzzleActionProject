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

    private bool m_isVisible;

    private Coroutine m_viewCoroutine;

    public void ViewHitCollider(AttackHitBox box)
    {
        m_currentHitBox = box;

        if (m_viewCoroutine != null) return;

        m_viewCoroutine = StartCoroutine(ViewColliderTime());
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

        Vector3 center = m_currentHitBox.m_pos + m_currentHitBox.m_hitBoxOffset;
        Gizmos.DrawWireSphere(center, m_currentHitBox.m_radius);
    }
}
