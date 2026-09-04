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
        Debug.Log("Trap Trigger");

        Entity target = other.GetComponent<Entity>();

        //Entityじゃない
        if (target == null)
        {
            return;
        }

        DamageData data = new DamageData();

        data.Attack = m_damage;

        data.Knockback = 0f;
        data.Stun = 0f;
        data.Duration = 0f;

        data.AttackDir = (target.transform.position - transform.position).normalized;

        data.Attacker = this;
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
        target.TakeDamage(data);
    }
}
