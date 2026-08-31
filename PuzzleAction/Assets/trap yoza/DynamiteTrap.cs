using UnityEngine;

public class DynamiteTrap : TrapBase
{
    protected override void SetUp()
    {

        //Debug.Log($"dynamite Setup");
        //if (m_owner != null)
        //{
        //    m_startPosition = m_owner.transform.position;
        //}
        //else
        //{
        //    m_startPosition = transform.position;
        //}

        //destroyRange null
    }

    protected override void OnHit()
    {
        
    }

    protected override void OnTriggerEnter(Collider other)
    {
        //if (!enabled) return;

        Entity target = other.GetComponentInParent<Entity>();

        if (target != null && target.Team != TeamType.Nature)
        {
            if (m_owner != null && target.Team == m_owner.Team) return;

            Debug.Log($"[DYNAMITE] {target.gameObject.name} が踏んだ！爆発！");

            // Cプールに戻す（あっちのTrapBaseに備わっているプール返却処理を呼ぶ）
            gameObject.SetActive(false);
        }
    }
    private void OnDrawGizmosSelected()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        if (box != null)
        {
            Gizmos.color = Color.green;
            Vector3 center = transform.position + transform.TransformDirection(box.center);
            Gizmos.DrawWireCube(center, box.size);
        }
    }
}   