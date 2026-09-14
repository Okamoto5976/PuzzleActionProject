using UnityEngine;

public class EnemyDemonController
{
    private enum DemonState
    {
        Chasing,
        Step,
        Attack
    }

    private enum StepDirection
    {
        Back,
        Left,
        Right
    }

    private enum ChasingAction
    {
        None,
        Meandering,
        Zigzag,
        Stop,
        Step
    }

    private EnemyController enemyController;
    private DemonState state;
    private StepDirection stepDir;
    private ChasingAction action;
    private Transform transform;
    private Rigidbody rb;

    private float time;

    private float probabilityOfTakeStep;
    private float nextActionDuration;
    private float stepPower;


    private float DistanceToTarget => Vector3.Distance(transform.position, enemyController.Target.Value);

    public void Initialize(EnemyController enemyController, Transform transform, float probablityOfTakeStep, float nextActionDuration, float stepPower)
    {
        this.enemyController = enemyController;
        this.transform = transform;
        this.probabilityOfTakeStep = probablityOfTakeStep;
        this.nextActionDuration = nextActionDuration;
        this.stepPower = stepPower;
        time = 0;
        enemyController.SetIsInvincible(true);
    }

    public void DoDemonStates()
    {
        time += Time.deltaTime;
        switch (state)
        {
            case DemonState.Chasing: DoChasing(); break;
            case DemonState.Attack: DoAttack(); break;
        }
    }

    private void DoChasing()
    {
        if (enemyController.Target == null) return;
        if (DistanceToTarget <= enemyController.AttackRange)
        {
            SetState(DemonState.Attack);
            return;
        }
    }

    private void DoAttack()
    {
        enemyController.Stop();
        enemyController.Attack();
        float lotteryTakeStep = Random.Range(0, 100);
        if (lotteryTakeStep < probabilityOfTakeStep)
        {
            SetState(DemonState.Step);
            return;
        }

        if (DistanceToTarget > enemyController.AttackRange)
        {
            SetState(DemonState.Chasing);
        }
    }



    private void TakeStep(StepDirection stepDir)
    {
        enemyController.SetEnableRotation(false);
        Vector3 stepForce = Vector3.zero;
        switch (stepDir)
        {
            case StepDirection.Back: stepForce = -Vector3.forward * stepPower + Vector3.up; break;
            case StepDirection.Left: stepForce = -Vector3.right * stepPower + Vector3.up; break;
            case StepDirection.Right: stepForce = Vector3.right * stepPower + Vector3.up; break;
        }
        rb.AddForce(stepForce);
    }

    private void AfterTheStep()
    {

    }

    private void SetState(DemonState state)
    {
        this.state = state;
        time = 0;
    }
}
