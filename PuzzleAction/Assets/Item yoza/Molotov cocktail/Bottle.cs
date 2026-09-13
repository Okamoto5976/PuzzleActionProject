using UnityEngine;

public class Bottle : TrapBase
{
    [Header("‰Š")]
    [SerializeField] private GameObject m_fireAreaPrefab;//‰Î

    [SerializeField] private float m_power;

    [SerializeField] private LayerMask m_hitLayers;

    private bool m_isInitialized;

    protected override void EntitySetUp()
    {
        m_rb.linearVelocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;
        m_isInitialized = true;
    }

    protected override void OnHit()
    {
        SpawnFlame();
        OnReturnPool();

    }

    private void FixedUpdate()
    {
        if (!m_isInitialized)
            return;

        OnAddForce(m_dir, m_power);

        m_isInitialized = false;
        //CheckRange();
    }

    private void Update()
    {
        CheckDeadLine();

    }

    protected override void OnTriggerEnter(Collider other)
    {
        if ((m_hitLayers.value & (1 << other.gameObject.layer)) != 0)
        {
            OnHit();
            return;
        }

        //Entity target = other.GetComponent<Entity>();

        //if(target != null)
        //{
        //    if (target.Team == m_team) return;
        //}

        //OnHit();

    }

    private void SpawnFlame()
    {
        if(m_fireAreaPrefab != null)
        {
            GameObject fireObj = Instantiate(m_fireAreaPrefab, transform.position, Quaternion.identity);

            Flame fireArea = fireObj.GetComponent<Flame>();
           
            if (fireArea!=null)
            {
             fireArea.InitFire(m_owner, m_team, m_str);
            }
        }
        OnReturnPool();
    }

}
