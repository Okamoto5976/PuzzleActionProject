using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BossEnemyHP : EntityHP
{
    private ReturnObjectToPool m_returnObjPool;
    [SerializeField] private bool m_isItemDrop;

    [SerializeField] private DropMoneyEventSO m_dropMoneyEventSO;

    protected override void Die()
    {
        BossEnemyController boss = GetComponent<BossEnemyController>();
        if (boss == null)
        {
            Debug.Log($"{this.name} : BossEnemyController not found");
            return;
        }
        if (boss.CurrentState == Entity.EntityState.Dead) return;

        //dropMoney
        m_dropMoneyEventSO.Raise(transform.position, 380);

        boss.KillEntity();
        boss.OnDead(m_isItemDrop);
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
