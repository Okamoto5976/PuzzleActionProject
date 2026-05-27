using UnityEngine;

public class TrapBase : Entity
{
    //方向
    protected Vector3 m_direction;

    //開始位置
    protected Vector3 m_startPosition;

    //使用者
    protected GameObject m_owner;

    //射程
    protected float m_range;

    //ダメージ
    protected int m_damage;


    protected override void FixedUpdate()
    {
        //移動方向
        m_moveDir = m_direction;

        //Entity移動
        base.FixedUpdate();

        //射程
        CheckRange();

    }

    //射程
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

        target.TakeDamage(m_damage);
        Debug.Log(other.name + "Hit");

        Destroy(gameObject);
        //後でpull
    }
}


