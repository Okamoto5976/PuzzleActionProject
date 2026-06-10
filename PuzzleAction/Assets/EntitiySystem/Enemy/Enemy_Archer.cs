using UnityEngine;

public class Enemy_Archer : MonoBehaviour, IEnemyBehaviour
{
    private EnemyContllor m_controller;
    
    //[SerializeField] private ItemManager m_itemManager;

    private float m_coolTime = 1.5f;
    private float m_lastFireTime;

    public void Initialized(EnemyContllor controller)
    {
        m_controller = controller;
    }

    public void Execute()
    {
        if (m_controller.Target == null) return;

        Vector3 dir = (m_controller.Target.position - transform.position).normalized;
        dir.y = 0f;

        transform.rotation = Quaternion.LookRotation(dir);

        TryShoot(dir);
    }

    private void TryShoot(Vector3 dir)
    {
        if (Time.time < m_lastFireTime + m_coolTime)
            return;

        m_lastFireTime = Time.time;

        Shoot(dir);
    }

    private void Shoot(Vector3 dir)
    {
        //call Itemmanager method
        //dir transform itemname
    }

    public void Stop()
    {

    }
}