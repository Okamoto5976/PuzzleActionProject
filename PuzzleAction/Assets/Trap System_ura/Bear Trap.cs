using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : TrapBase
{
    [Header("Recovery")]
    [SerializeField] private float m_recoveryTime = 3.0f;

    private bool m_isActive = true;

    private HashSet<Entity> m_targets = new HashSet<Entity>();

    private Coroutine m_recoveryCoroutine;
    private Coroutine m_damageCoroutine;

    [SerializeField] private AudioData m_se;
    private void Awake()
    {
        m_anim = GetComponentInChildren<Animator>();
    }

    protected override void EntitySetUp()
    {
        m_isActive = true;
        m_targets.Clear();

        if (m_recoveryCoroutine != null)
        {
            StopCoroutine(m_recoveryCoroutine);
            m_recoveryCoroutine = null;
        }

        if (m_damageCoroutine != null)
        {
            StopCoroutine(m_damageCoroutine);
            m_damageCoroutine = null;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!m_isActive)
        {
            return;
        }

        Entity target = other.GetComponentInParent<Entity>();

        if (target == null) return;
        if (target.Team == TeamType.Nature) return;
        if (target.Team == m_team) return;

        m_targets.Add(target);

        if (m_damageCoroutine == null)
        {
            m_damageCoroutine = StartCoroutine(DamageCoroutine());
        }

        m_isActive = false;

        if (m_recoveryCoroutine != null)
        {
            StopCoroutine(m_recoveryCoroutine);
        }

        m_recoveryCoroutine = StartCoroutine(RecoveryCoroutine());
    }

    private void OnTriggerExit(Collider other)
    {
        Entity target = other.GetComponentInParent<Entity>();

        if (target == null)
        {
            return;
        }

        m_targets.Remove(target);

        if (m_targets.Count == 0)
        {
            StopDamage();
        }
    }

    private IEnumerator DamageCoroutine()
    {
        m_anim.SetBool("Attack", true);

        while (m_targets.Count > 0)
        {
            List<Entity> removeTargets = new List<Entity>();


            foreach (Entity target in m_targets)
            {
                if (target == null)
                {
                    continue;
                }

                if (!target.gameObject.activeInHierarchy)
                {

                    removeTargets.Add(target);
                    continue;
                }


                target.TakeDamage(m_damageData);
            }


            foreach (Entity target in removeTargets)
            {
                m_targets.Remove(target);
            }

            OnHit();
            yield return new WaitForSeconds(1.0f);
        }


        m_damageCoroutine = null;
    }

    private void StopDamage()
    {
        m_anim.SetBool("Attack", false);

        if (m_damageCoroutine != null)
        {
            StopCoroutine(m_damageCoroutine);
            m_damageCoroutine = null;
        }
    }

    private IEnumerator RecoveryCoroutine()
    {
        yield return new WaitForSeconds(m_recoveryTime);

        m_isActive = true;
        m_recoveryCoroutine = null;
    }

    private void OnDisable()
    {
        StopDamage();

        if (m_recoveryCoroutine != null)
        {
            StopCoroutine(m_recoveryCoroutine);
            m_recoveryCoroutine = null;
        }

        m_targets.Clear();
    }

    protected override void OnHit()
    {
        AudioManager.Instance.PlayAudio(m_se);
    }

    public override void TrapInit()
    {
        base.TrapInit();

        StopDamage();

        if (m_recoveryCoroutine != null)
        {
            StopCoroutine(m_recoveryCoroutine);
            m_recoveryCoroutine = null;
        }

        m_isActive = true;
        m_targets.Clear();
    }
}