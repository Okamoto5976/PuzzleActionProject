using System.Collections;
using UnityEngine;

public class CaltropTrap : TrapBase
{
    [Header("Damage")]
    [SerializeField]
    private float m_damageInterval = 1.0f;


    private Entity m_target;

    private Coroutine m_damageCoroutine;


    protected override void SetUp()
    {
        // 初期化
        m_target = null;

        // 前回のCoroutineが残っていた場合
        if (m_damageCoroutine != null)
        {
            StopCoroutine(m_damageCoroutine);
            m_damageCoroutine = null;
        }
    }


    protected override void OnHit()
    {
        // TrapBaseの抽象メソッドなので
        // 必要に応じてヒット時の処理を書く
    }


    protected override void OnTriggerEnter(
        Collider other)
    {
        Entity target =
            other.GetComponent<Entity>();

        if (target == null)
            return;

        // 自分自身にはダメージを与えない
        if (target == m_owner)
            return;

        // 同じチームなら無視
        if (target.Team == m_team)
            return;

        m_target = target;

        // すでにダメージ処理中なら開始しない
        if (m_damageCoroutine == null)
        {
            m_damageCoroutine =
                StartCoroutine(
                    DamageCoroutine());
        }
    }


    private void OnTriggerExit(
        Collider other)
    {
        Entity target =
            other.GetComponent<Entity>();

        if (target == null)
            return;

        // 今ダメージを与えている対象が離れた
        if (target == m_target)
        {
            StopDamage();
        }
    }


    private IEnumerator DamageCoroutine()
    {
        while (m_target != null)
        {
            // ダメージ処理
            m_target.TakeDamage(
                m_damageData);

            // 一定時間待つ
            yield return new WaitForSeconds(
                m_damageInterval);
        }

        m_damageCoroutine = null;
    }


    private void StopDamage()
    {
        if (m_damageCoroutine != null)
        {
            StopCoroutine(
                m_damageCoroutine);

            m_damageCoroutine = null;
        }

        m_target = null;
    }


    private void OnDisable()
    {
        StopDamage();
    }
}