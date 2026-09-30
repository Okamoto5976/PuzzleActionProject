using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_HitCollider : MonoBehaviour
{
    private HitCollider m_hitCollider;
    [SerializeField] private bool m_isViewCollider;
    private bool m_isVisible;
    DamageData m_damageData;
    TeamType m_teamType;

    [SerializeField] private AttackHitBox m_currentHitBox;

    protected Coroutine m_viewCoroutine;

    private void Awake()
    {
        m_damageData = new();
        m_teamType = TeamType.Enemy;
        m_currentHitBox = new()
        {
            m_transform = transform,
            m_hitBoxOffset = Vector3.zero,
            m_radius = 0
        };
    }

    private void Update()
    {
        var current = Keyboard.current;

        // キーボード接続チェック
        if (current == null)
        {
            // キーボードが接続されていないと
            // Keyboard.currentがnullになる
            return;
        }

        var spaceKey = current.spaceKey;

        if (spaceKey.wasPressedThisFrame)
        {
            Debug.Log("spaceキーが押された！");
            m_hitCollider.AttackCollider(m_damageData, m_teamType, m_currentHitBox);
        }
    }
}
