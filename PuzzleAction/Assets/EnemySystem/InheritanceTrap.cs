using UnityEngine;

public class InheritanceTrap : Entity
{
    [Header("Trap")]
    [SerializeField]
    protected int m_damage = 10; //仮置き

    protected virtual void Reset()
    {
        //m_team = Teamtype.Neutral;

        //m_damageable = false;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Entity target = other.GetComponent<Entity>();

        //Entityじゃない
        if (target == null)
        {
            return;
        }

        //当たるか
        //if(!CanHit(target))
        //{
        //    return;
        //}

        //ダメージ可能か
        //if(!target.Damageable)
        //{
        //    return;
        //}

        //ダメージ
        target.TakeDamage(m_damage);
    }
}
