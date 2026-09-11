using UnityEngine;

public class Bottle : TrapBase
{
    [Header("‰Š")]
    [SerializeField] private GameObject m_fireAreaPrefab;//‰Î

    protected override void SetUp()
    {
        
    }
    protected override void OnHit()
    {
        SpawnFlame();
    }
    private void FixedUpdate()
    {
        OnMove(m_dir);
        CheckRange();
        CheckDeadLine();
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        Entity target = other.GetComponent<Entity>();

        if(target != null)
        {
            if (target.Team == m_team) return;
        }
        SpawnFlame();
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
