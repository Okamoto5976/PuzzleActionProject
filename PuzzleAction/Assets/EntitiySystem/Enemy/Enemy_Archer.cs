using UnityEngine;

public class Enemy_Archer : MonoBehaviour, IEnemyBehaviour
{
    private EnemyContllor m_controller;

    private Entity m_entity;

    //[SerializeField] private ItemManager m_itemManager;
    [SerializeField] private ArrowTrap m_arrowPrefab;

    private float m_coolTime = 1.5f;
    private float m_lastFireTime;

    private void Awake()
    {
        m_entity = GetComponent<Entity>();
    }

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

        Use(dir);
        //Instantiate(m_arrow, transform.position, new Quaternion(dir.x, dir.y, dir.z, 0));
    }

    private void Use(Vector3 dir)
    {
        ArrowTrap arrow = Instantiate(
            m_arrowPrefab
            );

        arrow.Init(m_entity, dir, 5);
        arrow.transform.position = transform.position;

        //TrapUseData data = new();

        ////使用者
        //data.Owner =
        //    this.gameObject;

        ////出現位置
        //data.Position =
        //    transform.position;

        ////使用方向
        //data.Direction = dir;
        ////m_owner.transform.forward;
        ////Debug.Log(m_owner.name);
        ////Debug.Log(m_owner.transform.forward);

        ////初期化
        //arrow.Initialize(data);
    }

    public void Stop()
    {

    }
}