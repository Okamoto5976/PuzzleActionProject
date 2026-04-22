using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ▼(仮)--------------------------
public enum TeamType 
{ 
    Player,
    Enemy,
    Item
};

public class DamageData
{
    public int Attack;
    public float Knockback;
    public Vector3 AttackDir;
    //public DamageType DamageType;
    public int Duration;
}
public class DamageResult
{
    public Vector3 hitPoint;
    public Vector3 hitNormal;

    //public EffectDataSO overrideEffectData = m_overrideEffect;
    //public AudioDataSO overrideAudioData = m_overrideAudio;
}
// ▲------------------------------

[System.Serializable]
public class AttackHitBox
{
    public Transform m_transform;
    public float m_radius;
}

public class HitCollider : MonoBehaviour
{

    private void Awake()
    {
        
    }

    [SerializeField] private bool m_isViewCollider;
    [SerializeField] private bool m_isVisible;

    
    [SerializeField] private AttackHitBox[] hitBoxes;   // 当たり判定

    //[SerializeField] private EffectDataSO m_overrideEffect;
    //[SerializeField] private AudioDataSO m_overrideAudio;

    private Coroutine m_viewCoroutine;
    
    public void AttackCollider(DamageData data, TeamType myTeam, AttackHitBox attackHitBox)
    {
        // ヒットした判定のセット
        HashSet<IDamage> hitSet = new HashSet<IDamage>();

        foreach (var hitBox in hitBoxes)
        {
            if (attackHitBox == hitBox) continue;
            if (hitBox.m_transform == null) continue;

            //Collider[] hits = Physics.OverlapSphere(
            //    hitBox.m_transform.position,
            //    hitBox.m_radius
            //);

            Transform[] hits = My_OverlapSphere(
                attackHitBox
            );
            
            //Debug.Log($"hits.Length : {hits.Length}");

            foreach (var hit in hits)
            {
                //Debug.Log($"hit : {hit}");
                var damageable = hit.GetComponentInParent<IDamage>();
                if (damageable == null) continue;
                if (hitSet.Contains(damageable)) continue;

                var team = hit.GetComponentInParent<ITeam>();

                //Debug.Log($"team.Team : {team.Team}");
                if (team != null)
                {
                    //Debug.Log($"myTeam : {myTeam}");
                    // 同じチームなら無視
                    if (team.Team == myTeam) continue;
                }
                else
                {
                    continue;
                }

                hitSet.Add(damageable);

                //Vector3 hitPoint = col.ClosestPoint(hitBox.m_transform.position);
                Vector3 hitPoint = My_ClosestPoint(hit.transform.position, hitBox.m_transform.position, hitBox.m_radius);
                Vector3 hitNormal = (hitPoint - hitBox.m_transform.position).normalized;
                //Debug.Log($"hitName : {hit.name} , hitPos : {hit.transform.position} , hitBoxPos : {hitBox.m_transform.position}");
                //Debug.Log($"hitPoint : {hitPoint}");

                DamageResult result = new DamageResult
                {
                    hitPoint = hitPoint,
                    hitNormal = hitNormal,

                    //overrideEffectData = m_overrideEffect,
                    //overrideAudioData = m_overrideAudio
                };

                damageable.TakeDamage(data, result);
            }

        }

        if (m_isViewCollider)
        {
            if (m_viewCoroutine != null) return;

            m_viewCoroutine = StartCoroutine(ViewColliderTime());
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
        //Debug.Log("DrawGizmos");

        Gizmos.color = Color.red;

        foreach(var hitBox in hitBoxes)
        {
            if (hitBox.m_transform == null) continue;
            Gizmos.DrawWireSphere(
                hitBox.m_transform.position,
                hitBox.m_radius
                );
        }
    }

    private Transform[] My_OverlapSphere(AttackHitBox attackHitBox)
    {
        List<Transform> colSet = new List<Transform>();
        foreach (var hitBox in hitBoxes)
        {
            if (hitBox.m_transform == null) continue;
            if (attackHitBox == hitBox) continue;

            if (Distance(hitBox.m_transform.position, attackHitBox.m_transform.position)
                < (attackHitBox.m_radius + hitBox.m_radius) * (attackHitBox.m_radius + hitBox.m_radius))
            {
                colSet.Add(hitBox.m_transform.GetComponentInParent<Transform>());
            }
            else
            {
                continue;
            }
        }
        Transform[] col = colSet.ToArray();

        return col;
    }

    private Vector3 My_ClosestPoint(Vector3 pos1, Vector3 pos2, float rad2)
    {
        Vector3 closestPoint;
        //closestPoint = pos1 + (pos2 - pos1) * (rad1 / (rad1 + rad2));
        float distance = Mathf.Sqrt(Distance(pos1, pos2));
        closestPoint = pos1 + (pos2 - pos1) * ((distance - rad2) / distance);
        return closestPoint;
    }

    private float Distance(Vector3 pos1, Vector3 pos2)
    {
        float distance = (pos2.x - pos1.x) * (pos2.x - pos1.x)
                       + (pos2.y - pos1.y) * (pos2.y - pos1.y)
                       + (pos2.z - pos1.z) * (pos2.z - pos1.z);
        return distance;
    }
}
