using UnityEngine;

public class TrapBase : Entity
{
    //direction
    protected Vector3 m_direction;

    //startPosition
    protected Vector3 m_startPosition;

    //user entity
    protected Entity m_owner;

    //range
    protected float m_range = 10f;

    //basevalue
    protected float m_basevalue;

    //Receive orientation

    private ReturnObjectToPool rotp;


    protected override void Awake()
    {
        base.Awake();
        //Fix the rotation
        m_rb.constraints = RigidbodyConstraints.FreezeRotation;
        rotp = GetComponent<ReturnObjectToPool>();

    }
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
              m_owner.transform.position,
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
        Debug.Log($"Arrow Hit : {other.name}");
        //layer check

        Entity target = other.GetComponent<Entity>();
        if(target == null)
        {
            return;
        }

        //if nature can takeDamage , this return delete
        if (target.Team == TeamType.Nature) return;

        if(target.Team == m_owner.Team)
        {
            return;
        }

        //target.TakeDamage(STR + m_basevalue);
        Debug.Log(other.name + "Hit");

        Destroy(gameObject);
        //���poolreturn;
    }
}


