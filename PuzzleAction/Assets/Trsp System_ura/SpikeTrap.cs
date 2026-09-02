using UnityEngine;

public class SpikeTrap:TrapBase
{
    protected override void SetUp()
    {
        
    }
    protected override void OnHit()
    {
        
    }

    protected override void OnTriggerEnter(Collider other)
    {
        Entity entity = 
            other.GetComponent<Entity>();

        if (entity == null) 
        {
            return;
        }

        if(entity == m_owner)
        {
            return;
        }

        entity.TakeDamage(m_damageData);

        OnHit();
    }
}