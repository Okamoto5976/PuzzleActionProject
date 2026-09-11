using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingArea : MonoBehaviour
{
    [Header("回復設定")]
    [SerializeField] private float m_healAmount = 5f;
    [SerializeField] private float m_healInterval = 1f;

    private List<Entity> m_targets = new List<Entity>();
    private Coroutine m_healCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        Entity target = other.GetComponent<Entity>();
        if (target == null) return;

        if (!m_targets.Contains(target))
        {
            m_targets.Add(target);
        }

        // 誰かが入ってきたら、コルーチンが動いていない場合に開始
        if (m_healCoroutine == null && m_targets.Count > 0)
        {
            m_healCoroutine = StartCoroutine(HealRoutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity target = other.GetComponent<Entity>();
        if (target == null) return;

        m_targets.Remove(target);

        // 誰もいなくなったらコルーチンを止める
        if (m_targets.Count == 0 && m_healCoroutine != null)
        {
            StopCoroutine(m_healCoroutine);
            m_healCoroutine = null;
        }
    }

    private IEnumerator HealRoutine()
    {
        while (m_targets.Count > 0)
        {
            yield return new WaitForSeconds(m_healInterval);

            // null になったターゲットを掃除するためのリスト
            List<Entity> nullTargets = null;

            foreach (Entity target in m_targets)
            {
                if (target == null)
                {
                    if (nullTargets == null) nullTargets = new List<Entity>();
                    nullTargets.Add(target);
                    continue;
                }

                target.HealHP(m_healAmount);
            }

            // 破壊されたオブジェクトをリストから除外
            if (nullTargets != null)
            {
                foreach (var t in nullTargets)
                {
                    m_targets.Remove(t);
                }
            }
        }

        m_healCoroutine = null;
    }

    private void OnDisable()
    {
        if (m_healCoroutine != null)
        {
            StopCoroutine(m_healCoroutine);
            m_healCoroutine = null;
        }
        m_targets.Clear();
    }
}