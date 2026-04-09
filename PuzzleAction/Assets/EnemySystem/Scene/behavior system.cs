using UnityEngine;

public class behaviorsystem : MonoBehaviour
{
    private State state;
    private behaviorsystem movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = GetComponent<State>();
        movement = GetComponent<behaviorsystem>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleBehaavior();
    }

    //ó‘Ô‚É‰‚¶‚½‹““®§Œä
    void HandleBehaavior()
    {
        if (state == null || movement == null) return;

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
                HandheDamage();
                break;

            case State.EnemyState.Dead:
                HandheDead();
                break;
        }
    }

    //Šeó‘Ô‚Ìˆ—
    void HandleIdle()
    {
        
    }

    void HandleChase()
    {
        if (state.canMove)
        {
            //movement.Move();

        }

    
    }

    void HandleAttack()
    {
        
        if (state.canAttack)
        {
            Debug.Log("UŒ‚ˆ—");
        }
    }

    void HandheDamage()
    {
    
    }

    void HandheDead()
    {
       
        Debug.Log("€–Só‘Ô");
    }
}
