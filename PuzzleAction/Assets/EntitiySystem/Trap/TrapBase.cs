using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class TrapBase : Entity
{
    //direction
    protected Vector3 m_direction;

    //startPosition
    protected Vector3 m_startPosition;

    //user
    protected GameObject m_owner;

    //range
    protected float m_range;

    //damage
    protected int m_damage;

    //basevalue
    protected int m_basevalue;

    //Receive orientation
    public void SetDirection(Vector3 direction)
    {
        m_direction = direction.normalized;
    }

    protected override void FixedUpdate()
    {
        //movement
        m_moveDir = m_direction;

        //Entitymovement
        base.FixedUpdate();

        //range
        CheckRange();
    }

    //range
    private void CheckRange()
    {
        float distance =
          Vector3.Distance(
              m_startPosition,
              transform.position
              );
        if (distance >= m_range)
        {
            Destroy(gameObject);
        }
    }

    //Hit
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == m_owner)
        {
            return;
        }

        Entity target=
            other.GetComponent<Entity>();

        if(target == null)
        {
            return;
        }

        Entity ownerEntity=
            m_owner.GetComponent<Entity>();

        if (ownerEntity != null)
        {
            if (target.Team == ownerEntity.Team)
            {
                return;
            }
        }

        //target.TakeDamage(m_damage);
        Debug.Log(other.name + "Hit");

        Destroy(gameObject);
        //Œã‚Åpull
    }
}


