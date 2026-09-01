using System.Collections;
using UnityEngine;

public class BossGolemAttack : MonoBehaviour
{
    [Header("Rock")]
    [SerializeField] private float m_rockSpawnY = 8f;

    [Header("Stomp")]
    [Header("ShockWave")]
    [SerializeField] private StompShockWave m_shockWavePrefab;
    private float m_shockWaveRadius;
    [SerializeField] private float m_stompDelay = 2f;
    [SerializeField] private HitCollider m_hitCollider;
    [SerializeField] private AttackHitBox m_stompHitBox;

    private BossEnemyController m_controller;
    private Rigidbody m_rockRB;
    private bool m_isAttack;

    public void Initialize(BossEnemyController controller)
    {
        m_controller = controller;
        m_shockWaveRadius = m_controller.AttackRange;
    }

    public void StartRockThrow()
    {
        if (m_isAttack) return;

        StartCoroutine(RockThrowCoroutine());
    }

    public void StartStomp()
    {
        if (m_isAttack) return;

        StartCoroutine(StompCoroutine());
    }

    IEnumerator RockThrowCoroutine()
    {
        m_isAttack = true;

        m_controller.Stop();

        yield return new WaitForSeconds(1f);

        Vector3 pos = transform.position + Vector3.up * 2f;
        ///pos.y += m_rockSpawnY;

        Vector3 dir =(m_controller.Target.Value - transform.position).normalized;

        ItemRecieveData data =new ItemRecieveData();
        data.entity = m_controller;
        data.baseValue = m_controller.STR;
        data.pos = pos;
        data.dir = dir;
        data.size = Vector2.one;

        Debug.Log($"Rock Spawn Request : {pos}");
        Debug.Log("Rock Spawn");
        m_controller.UseItem(data);

        yield return new WaitForSeconds(0.5f);

        m_isAttack = false;
        m_controller.EndAttack();
    }

    IEnumerator StompCoroutine()
    {
        m_isAttack = true;

        m_controller.Stop();

        Vector3 startPos = transform.position;
        Vector3 jumpPos = startPos + Vector3.up * 5f;

        float timer = 0f;

        // Jump
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos,jumpPos,timer / 0.5f);
            yield return null;
        }

        timer = 0f;

        // Fall
        while (timer < 0.3f)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(jumpPos,startPos,timer / 0.3f);
            yield return null;
        }

        CreateShockWave();

        yield return new WaitForSeconds(m_stompDelay);
        m_isAttack = false;
        m_controller.EndAttack();
    }

    private void CreateShockWave()
    {
        DamageData damage =
            new DamageData
            {
                Attack = (int)m_controller.STR,
                CriticalRate = m_controller.CriticalRate,
                CriticalDamage = m_controller.CriticalDamage,
                BreakRate = m_controller.BreakRate,
                Knockback = m_controller.KnockBack,
                Stun = m_controller.Stun,
                AttackDir = transform.forward,
                Attacker = m_controller,
                AttackerSE = m_controller.AttackSE,
                AudioSource = m_controller.AudioSource
            };

        StompShockWave shockWave = Instantiate(m_shockWavePrefab,transform.position,Quaternion.identity);
        shockWave.transform.localScale =Vector3.one * m_shockWaveRadius;
        shockWave.Initialize(damage,m_controller.Team, m_shockWaveRadius,0.5f);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
        {
            if (m_stompHitBox == null) return;
            if (m_stompHitBox.m_transform == null) return;

            Gizmos.color = new Color(1f,0f,0f,0.5f);
            Gizmos.DrawWireSphere(m_stompHitBox.m_transform.position,m_stompHitBox.m_radius);
        }
    #endif



    #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (m_controller == null) return;

            if (m_controller.Target == null) return;

            Vector3 pos = m_controller.Target.Value;
            pos.y += m_rockSpawnY;

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(pos, 0.3f);
            Gizmos.DrawLine(transform.position,pos);
        }
    #endif
}