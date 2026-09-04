using UnityEngine;

public class EnemyHP : EntityHP
{
    private ReturnObjectToPool m_returnObjPool;

    protected override void Die()
    {
        //Ž€‚ñ‚¾‚Æ‚«pool‚É–ß‚é
        // player get money
        // add score
        // item drop
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
