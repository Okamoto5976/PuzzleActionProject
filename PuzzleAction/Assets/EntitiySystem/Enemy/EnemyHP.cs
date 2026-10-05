using UnityEngine;

public class EnemyHP : EntityHP
{
    private ReturnObjectToPool m_returnObjPool;
    [SerializeField] private bool m_isItemDrop;

    [SerializeField] private DropMoneyEventSO m_dropMoneyEventSO;

    protected override void Die()
    {
        EnemyController enemy = GetComponent<EnemyController>();
        if(enemy == null)
        {
            Debug.Log($"{this.name} : EnemyController not found");
            return;
        }

        if (enemy.CurrentState == Entity.EntityState.Dead) return;

        //dropMoney
        m_dropMoneyEventSO.Raise(transform.position, 160);

        //kill enemy
        enemy.KillEntity();
        //item drop from enemy
        enemy.OnDead(m_isItemDrop);
        //return pool 
        OnReturnPool();
    }
    private void OnReturnPool()
    {
        if (m_returnObjPool == null)
        {
            m_returnObjPool = GetComponent<ReturnObjectToPool>();

        }
        m_returnObjPool.ReturnToPool();

        //EnemyController enemy = GetComponent<EnemyController>();
        //enemy.ChangeState(Entity.EntityState.Idle);
        //m_entity.HealHP(m_entity.HP);

        Debug.Log("EnemyReturnPool");

    }
}
