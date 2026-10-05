using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ReturnObjectToPool))]
public class PoisonMagic : TrapBase
{
    [Header("child objctma")]
    [SerializeField] private GameObject m_bulletObject;
    [SerializeField] private GameObject m_areaObjct;

    [Header("Direct Hit Setting")]
    [SerializeField] private LayerMask m_hitLayers;
    [SerializeField] private float m_lifeTime = 5f;

    [SerializeField] private float m_duration = 10f;
    [SerializeField] private float m_tickInterval = 0.5f;
    [SerializeField] private float m_poisonDuration = 4f;

    private bool m_isAreaActive = false;
    private float m_flyTimer = 0f;
    private float m_areaTimer = 0f;
    private float m_tickTimer = 0f;

    private readonly List<Entity> m_targetsInRange = new List<Entity>();

    private void Awake()
    {
        // エラー帽子
        if (m_rb == null)
        {
            m_rb = GetComponent<Rigidbody>();
        }
    }

    protected override void EntitySetUp()
    {
        m_isAreaActive = false;
        m_flyTimer = 0f;
        m_areaTimer = 0f;
        m_tickTimer = 0f;
        m_targetsInRange.Clear();

        if (m_bulletObject != null) m_bulletObject.SetActive(true);
        if (m_areaObjct != null) m_areaObjct.SetActive(false);

        if (m_rb != null)
        {
            m_rb.isKinematic = false;
            m_rb.useGravity = false;
            m_rb.linearVelocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if (!m_isAreaActive && m_rb != null)
        {
            OnMove(m_dir);
        }
    }

    private void Update()
    {
        CheckDeadLine();

        if (!m_isAreaActive)
        {
            m_flyTimer += Time.deltaTime;
            if (m_flyTimer >= m_lifeTime)
            {
                OnReturnPool();
            }
            return;
        }

        m_areaTimer += Time.deltaTime;
        if (m_areaTimer >= m_duration)
        {
            OnReturnPool();
            return;
        }

        m_tickTimer += Time.deltaTime;
        if (m_tickTimer >= m_tickInterval)
        {
            m_tickTimer = 0f;
            ApplyPoisonEffect();
        }
    }

    protected override void OnHit()
    {
        StartPoisonArea();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!m_isAreaActive)
        {
            if ((m_hitLayers.value & (1 << other.gameObject.layer)) != 0)
            {
                OnHit();
                return;
            }

            Entity hitTarget = other.GetComponentInParent<Entity>();
            if (hitTarget != null && hitTarget.Team != m_team)
            {
                hitTarget.TakeDamage(m_damageData);
                OnHit();
            }
            return;
        }
        Entity inAreaTarget = other.GetComponentInParent<Entity>();
        if (inAreaTarget == null || inAreaTarget.Team == m_team) return;

        if (!m_targetsInRange.Contains(inAreaTarget))
        {
            m_targetsInRange.Add(inAreaTarget);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!m_isAreaActive) return;

        Entity target = other.GetComponentInParent<Entity>();
        if (target != null && m_targetsInRange.Contains(target))
        {
            m_targetsInRange.Remove(target);
        }
    }

    private void StartPoisonArea()
    {
        m_isAreaActive = true;

        if (m_rb != null)
        {
            m_rb.linearVelocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
            m_rb.isKinematic = true;
        }

        if(m_bulletObject != null) m_bulletObject.SetActive(false);
        if(m_areaObjct!=null)m_areaObjct.SetActive(true);

        DetectInitialTargets();

    }
    private void DetectInitialTargets()
    {
        if (m_areaObjct == null) return;

        // エリア子オブジェクトの位置・コライダーを参照して初期接触を検知
        Collider areaCol = m_areaObjct.GetComponent<Collider>();
        if (areaCol == null) return;

        Collider[] hitColliders = Physics.OverlapSphere(areaCol.bounds.center, m_areaObjct.transform.localScale.x * 0.5f);

        foreach (var col in hitColliders)
        {
            Entity target = col.GetComponentInParent<Entity>();
            if (target != null && target.Team != m_team)
            {
                if (!m_targetsInRange.Contains(target))
                {
                    m_targetsInRange.Add(target);
                }
            }
        }
    }


    private void ApplyPoisonEffect()
    {
        for (int i = m_targetsInRange.Count - 1; i >= 0; i--)
        {
            Entity target = m_targetsInRange[i];

            if (target == null)
            {
                m_targetsInRange.RemoveAt(i);
                continue;
            }

            if (m_trapData != null && m_trapData.m_buffSetting != null && m_trapData.m_buffSetting.Count != 0)
            {
                foreach (var buff in m_trapData.m_buffSetting)
                {
                    if (buff.m_duration <= 0) continue;

                    var modifier = SetModifier(buff);
                    target.AddBuff(modifier, buff.m_buffID, buff.m_duration);
                }
            }
        }
    }
}