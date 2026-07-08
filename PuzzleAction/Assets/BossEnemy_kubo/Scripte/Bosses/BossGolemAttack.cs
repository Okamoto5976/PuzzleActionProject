using System.Collections;
using UnityEngine;

public class BossGolemAttack : MonoBehaviour
{
    [Header("Rock")]
    [SerializeField] private float m_rockSpawnY = 8f;

    [Header("Stomp")]
    [SerializeField] private float m_stompDelay = 2f;
    [SerializeField] private float m_stompRadius = 5f;
    [SerializeField] private HitCollider m_hitCollider;
    [SerializeField] private AttackHitBox m_stompHitBox;

    private BossEnemyController m_controller;

    private bool m_isAttack;

    public void Initialize(BossEnemyController controller)
    {
        m_controller = controller;
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

        Vector3 pos = m_controller.Target.Value;
        pos.y += m_rockSpawnY;

        Vector3 dir =(m_controller.Target.Value - transform.position).normalized;

        ItemRecieveData data =new ItemRecieveData();

        data.entity = m_controller;
        data.baseValue = m_controller.STR;
        data.pos = pos;
        data.dir = dir;
        data.size = Vector2.one;

        m_controller.UseItem(data);

        yield return new WaitForSeconds(0.5f);

        m_isAttack = false;

        m_controller.EndAttack();
    }

    IEnumerator StompCoroutine()
    {
        m_isAttack = true;

        m_controller.Stop();

        // TODO:

        yield return new WaitForSeconds(m_stompDelay);

        DamageData damage = new DamageData();

        damage.Attack = (int)m_controller.STR;
        damage.HitRate = 100;
        damage.CriticalRate = m_controller.CriticalRate;
        damage.CriticalDamage = m_controller.CriticalDamage;
        damage.BreakRate = m_controller.BreakRate;
        damage.Knockback = m_controller.KnockBack;
        damage.Stun = m_controller.Stun;
        damage.AttackDir = transform.forward;
        damage.Attacker = m_controller;
        damage.AttackerSE = m_controller.AttackSE;
        damage.AudioSource = m_controller.AudioSource;

        m_hitCollider.AttackCollider(damage, m_controller.Team, m_stompHitBox);

        yield return new WaitForSeconds(1f);

        m_isAttack = false;

        m_controller.EndAttack();
    }
}