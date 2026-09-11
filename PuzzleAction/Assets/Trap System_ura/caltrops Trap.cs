using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaltropTrap : TrapBase
{
    [Header("Damage")]
    [SerializeField]
    private float m_damageInterval = 1.0f;


    // 現在、まきびしの範囲内にいるEntity
    private HashSet<Entity> m_targets =
        new HashSet<Entity>();


    private Coroutine m_damageCoroutine;


    protected override void SetUp()
    {
        // 初期化
        m_targets.Clear();

        if (m_damageCoroutine != null)
        {
            StopCoroutine(m_damageCoroutine);
            m_damageCoroutine = null;
        }
    }


    protected override void OnHit()
    {
        // 継続ダメージなので、
        // 特別なHit処理はここでは不要
    }


    protected override void OnTriggerEnter(Collider other)
    {
        Entity target =
            other.GetComponent<Entity>();

        if (target == null)
            return;


        // 自分自身にはダメージを与えない
        if (target == m_owner)
            return;


        // 自分と同じTeamならダメージを与えない
        if (target.Team == m_team)
            return;


        // 対象を追加
        m_targets.Add(target);


        // Coroutineが動いていなければ開始
        if (m_damageCoroutine == null)
        {
            m_damageCoroutine =
                StartCoroutine(DamageCoroutine());
        }
    }


    private void OnTriggerExit(Collider other)
    {
        Entity target =
            other.GetComponent<Entity>();

        if (target == null)
            return;


        // 範囲から出たEntityを削除
        m_targets.Remove(target);


        // 誰もいなくなったら停止
        if (m_targets.Count == 0)
        {
            StopDamage();
        }
    }


    private IEnumerator DamageCoroutine()
    {
        while (m_targets.Count > 0)
        {
            // 現在範囲内にいる全Entityにダメージ
            foreach (Entity target in m_targets)
            {
                if (target == null)
                    continue;

                target.TakeDamage(m_damageData);
            }


            // 次のダメージまで待つ
            yield return new WaitForSeconds(
                m_damageInterval);
        }


        m_damageCoroutine = null;
    }


    private void StopDamage()
    {
        if (m_damageCoroutine != null)
        {
            StopCoroutine(m_damageCoroutine);
            m_damageCoroutine = null;
        }
    }


    private void OnDisable()
    {
        StopDamage();
        m_targets.Clear();
    }
}