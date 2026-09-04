using UnityEngine;

public class EnemySpearController
{
    private enum SpearStates 
    {
        Chasing,
        Ready,
        Attack
    }

    private EnemyController enemyController;
    private SpearStates state;
    private Transform transform;

    private float time;

    private float preAttackDuration;
    private float postAttackDuration;

    private bool hasAttacked = false;

    private float DistanceToTarget => Vector3.Distance(transform.position, enemyController.Target.Value);


    public void Initialize(EnemyController enemyController, Transform transform, float preAttackDuration, float postAttackDuration)
    {
        this.enemyController = enemyController;
        this.transform = transform;
        this.preAttackDuration = preAttackDuration;
        this.postAttackDuration = postAttackDuration;
        time = 0;
        enemyController.SetIsInvincible(true);
    }

    public void DoSpearStates()
    {
        time += Time.deltaTime;
        switch (state)
        {
            case SpearStates.Chasing: DoChasing();  break;
            case SpearStates.Ready: DoReady();  break;
            case SpearStates.Attack: DoAttack();  break;
        }
    }

    private void DoChasing()
    {
        if (enemyController.Target == null) return;
        if (DistanceToTarget <= enemyController.AttackRange)
        {
            SetState(SpearStates.Ready);
            enemyController.SetIsInvincible(true);
            return;
        }

        enemyController.SetDestination(enemyController.Target.Value, enemyController.Speed);
    }

    private void DoReady()
    {
        enemyController.Stop();
        if (DistanceToTarget > enemyController.AttackRange)
        {
            SetState(SpearStates.Chasing);
            enemyController.SetIsInvincible(true);
            return;
        }
        if (enemyController.IsCooldownReady)
        {
            SetState(SpearStates.Attack);
            enemyController.SetIsInvincible(false);
            //enemyController.SetEnableRotation(false)
        }
    }

    private void DoAttack()
    {
        if (time >= preAttackDuration && !hasAttacked)
        {
            hasAttacked = true;
            enemyController.Attack();
        }
        if (time >= preAttackDuration + postAttackDuration)
        {
            SetState(SpearStates.Ready);
            enemyController.SetIsInvincible(true);
            //enemyController.SetEnableRotation(true)
        }
    }

    private void SetState(SpearStates state)
    {
        this.state = state;
        time = 0;
        hasAttacked = false;
    }

}
