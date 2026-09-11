using UnityEngine;

public class EnemyHP : EntityHP
{
    private ReturnObjectToPool m_returnObjPool;
    [SerializeField] private bool m_isItemDrop;

    protected override void Die()
    {
        //Ž€‚ñ‚¾‚Æ‚«pool‚É–ß‚é
        // player get money
        // add score
        // item drop
        EnemyController enemy = GetComponent<EnemyController>();
        if(enemy == null)
        {
            Debug.Log($"{this.name} : EnemyController not found");
            return;
        }
        enemy.OnDead(m_isItemDrop);
        OnReturnPool();
    }
    private void OnReturnPool()
    {
        if (m_returnObjPool == null)
        {
            m_returnObjPool = GetComponent<ReturnObjectToPool>();

        }
        m_returnObjPool.ReturnToPool();
        Debug.Log("EnemyReturnPool");

    }
}
