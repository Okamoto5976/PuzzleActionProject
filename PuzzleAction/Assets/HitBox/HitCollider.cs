using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

// ▼(仮)--------------------------
public enum TeamType 
{ 
    Player,
    Enemy,
    Item
};

public class DamageData{ }
public class DamageResult
{
    public Vector3 hitPoint;
    public Vector3 hitNormal;

    //public EffectDataSO overrideEffectData = m_overrideEffect;
    //public AudioDataSO overrideAudioData = m_overrideAudio;
}
// ▲------------------------------

public class HitCollider : MonoBehaviour
{

    private void Awake()
    {
        
    }

    [SerializeField] private bool m_isViewCollider;
    [SerializeField] private bool m_isVisible;

    [System.Serializable]
    public class AttackHitBox
    {
        public Transform m_pos;
        public float m_radius;
    }
    [SerializeField] private AttackHitBox[] m_hitBoxes;

    //[SerializeField] private EffectDataSO m_overrideEffect;
    //[SerializeField] private AudioDataSO m_overrideAudio;

    private Coroutine m_viewCoroutine;
    
    public void AttackCollider(DamageData data, TeamType myTeam)
    {
        // ヒットした判定のセット
        HashSet<IDamage> hitSet = new HashSet<IDamage>();

        foreach (var hitBox in m_hitBoxes)
        {
            if (hitBox.m_pos == null) continue;
            Debug.Log($"m_hitBoxes.Length : {m_hitBoxes.Length}");
            //Debug.Log($"hitBox.pos : {hitBox.m_pos.position}");
            //Debug.Log($"hitBox.rad : {hitBox.m_radius}");

            Collider[] hits = Physics.OverlapSphere(
                hitBox.m_pos.position,
                hitBox.m_radius
            );

            foreach (var col in hits)
            {
                Debug.Log($"col : {col}");
                Debug.Log($"col.name : {col.name}");
                var damageable = col.GetComponentInParent<IDamage>();
                if (damageable == null) continue;
                if (hitSet.Contains(damageable)) continue;

                var team = col.GetComponentInParent<ITeam>();
                Debug.Log($"team : {team}");
                //Debug.Log($"team.Team : {team.Team}");
                if (team != null)
                {
                    Debug.Log("team != null");
                    // 同じチームなら無視
                    if (team.Team == myTeam) continue;
                }
                else
                {
                    continue;
                }

                hitSet.Add(damageable);
                Debug.Log("Damage");

                Vector3 hitPoint = col.ClosestPoint(hitBox.m_pos.position);
                Vector3 hitNormal = (hitPoint - hitBox.m_pos.position).normalized;

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
        Debug.Log("ViewColliderTime");
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

        foreach(var hitBox in m_hitBoxes)
        {
            if (hitBox.m_pos == null) continue;
            Gizmos.DrawWireSphere(
                hitBox.m_pos.position,
                hitBox.m_radius
                );
        }
    }
}
