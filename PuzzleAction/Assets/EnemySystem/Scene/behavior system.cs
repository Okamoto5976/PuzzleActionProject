using UnityEngine;

public class Behaviorsystem : MonoBehaviour
{

    private State state;
    //private EnemyMovement movement; //EnemyMovement‚Ì•”•ª‚ÍAI@Behavior=UŒ‚ movement=ˆÚ“®

    [SerializeField] private GameObject attackCollider;
    [SerializeField] private float attackTime = 0.3f;   //UŒ‚‘±ŠÔİ’è

    public EnemyData data;

    private void Awake()
    {
        state = GetComponent<State>();
        //movement = GetComponent<EnemyMovement>(); //EnemyMovement‚Ì•”•ª‚ÍAI
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        HandleBehavior();

    }

    //ó‘Ô‚É‰‚¶‚½‹““®§Œä
    private void HandleBehavior()
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
    private void HandleIdle()
    {

    }

    //’ÇÕ
    private void HandleChase()
    {
        if (state.CanMove)
        {
            //movement.Move();
            //EnemyAISystem
            //Attack‚ğŒÄ‚Ô
        }
    }

    //UŒ‚
    private void HandleAttack()
    {

        if (state.CanAttack)
        {
            Attack();
        }
    }

    //UŒ‚uŠÔ
    private void Attack()
    {
        if (data.MoveAttack == EnemyData.AttackType.HitCollider)
        {
            Debug.Log("UŒ‚");

            attackCollider.SetActive(true);

            Invoke(nameof(EndAttack), attackTime);
        }
        //else if Ray
    }

    //UŒ‚I—¹
    private void EndAttack()
    {
        attackCollider.SetActive(false);
        Debug.Log("UŒ‚I—¹");
    }
    //ƒ_ƒ[ƒW
    private void HandleDamage()
    {
        //Damage State
        //HP
    }

    //€–S
    private void HandleDead()
    {
       //State•ÏX
        Debug.Log("€–Só‘Ô");
    }
}
