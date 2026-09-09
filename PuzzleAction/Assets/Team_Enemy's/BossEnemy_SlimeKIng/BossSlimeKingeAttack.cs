using System.Collections;
using UnityEngine;

public class BossSlimeKingeAttack : MonoBehaviour
{
    [Header("Summon")]
    //[SerializeField] private EnemyType m_Type;
    [SerializeField] private GameObject m_summonPrefab;

    private float m_SummonRadius;

    [SerializeField] private HitCollider m_hitCollider;
    [SerializeField] private AttackHitBox m_attackHitBox;

    private BossEnemyController m_controller;

    //private bool 
    public void Initalize(BossEnemyController controller)
    {
        m_controller = controller;
        m_SummonRadius = m_controller.AttackRange;
    }
    public void StartSummon()
    {
    //    if (m_controller.IsAttacking) return;
    //    StartCoroutine(SummonCoroutine());
    }
    public void StartTackle()
    {
    //    if (m_controller.IsAttacking) return;
        //    StartCoroutine(TackleCoroutine());
        //}
    
    }
    //IEnumerator SummonCoroutine()
    //{
        //    Vector3 Radius =transform.position;
        //    float radius = Random.Range(0, m_SummonRadius);
        //    float angle= Random.Range(0, 360);
        //    if(m_SummonRadius > radius)
        //    {
        //        Vector3 summon_pos = new Vector3(Radius.x + radius * Mathf.Cos(angle), Radius.y, Radius.z + radius * Mathf.Sin(angle));
        //        Instantiate(m_summonPrefab, summon_pos, Quaternion.identity);


    //    m_controller.EndAttack();
    //}

}


