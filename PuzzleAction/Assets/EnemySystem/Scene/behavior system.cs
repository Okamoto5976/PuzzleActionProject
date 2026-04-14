using UnityEngine;

public class Behaviorsystem : MonoBehaviour
{

    private State state;
    //private EnemyMovement movement; //EnemyMovement‚Ì•”•ª‚ÍAI@Behavior=UŒ‚ movement=ˆÚ“®

    [SerializeField] private GameObject attackCollider;
    [SerializeField] private float attackTime = 0.3f;

    public EnemyData data;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = GetComponent<State>();
        //movement = GetComponent<EnemyMovement>(); //EnemyMovement‚Ì•”•ª‚ÍAI
    }

    // Update is called once per frame
    void Update()
    {
        HandleBehavior();

    }

    //ó‘Ô‚É‰‚¶‚½‹““®§Œä
    void HandleBehavior()
    {
        if (state == null /*|| movement == null*/ ) return;  //movement‚Ì•”•ª‚ÍAI

        switch (state.currentState)
        {
            case State.EnemyState.Idle:
                HandleIdle();
                break;

            case State.EnemyState.Chase:
                HandleChase();
                break;

            case State.EnemyState.Attack:
                HandleAttack();
                break;

            case State.EnemyState.Damage:
                HandleDamage();
                break;

            case State.EnemyState.Dead:
                HandleDead();
                break;
        }
    }

    //Šeó‘Ô‚Ìˆ—
    void HandleIdle()
    {

    }

    //’ÇÕ
    void HandleChase()
    {
        if (state.canMove)
        {
            //movement.Move();

        }
    }

    //UŒ‚
    void HandleAttack()
    {

        if (state.canAttack)
        {
            Attack();
        }
    }

    //UŒ‚uŠÔ
    void Attack()
    {
        if (data.m_attackType == EnemyData.AttackType.HitCollider)
        {
            Debug.Log("UŒ‚");

            attackCollider.SetActive(true);

            Invoke(nameof(EndAttack), attackTime);
        }
    }

    //UŒ‚I—¹
    void EndAttack()
    {
        attackCollider.SetActive(false);
        Debug.Log("UŒ‚I—¹");
    }
    //ƒ_ƒ[ƒW
    void HandleDamage()
    {
    
    }

    //€–S
    void HandleDead()
    {
       
        Debug.Log("€–Só‘Ô");
    }
}
