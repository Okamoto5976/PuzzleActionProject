using System;
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
        Right,
        Max
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
    [SerializeField] private DemonState state;
    [SerializeField] private StepDirection stepDir;
    [SerializeField] private ChasingAction action;
    private Transform transform;
    private Rigidbody rb;

    [SerializeField] private float time;

    [SerializeField] private float probabilityOfTakeStep;
    [SerializeField] private float nextActionDuration;
    [SerializeField] private float stepPower;
    private bool wasStep;


    private float DistanceToTarget => Vector3.Distance(transform.position, enemyController.Target.Value);

    public void Initialize(EnemyController enemyController, Transform transform, float probablityOfTakeStep, float nextActionDuration, float stepPower, Rigidbody rb)
    {
        this.enemyController = enemyController;
        this.transform = transform;
        this.probabilityOfTakeStep = probablityOfTakeStep;
        this.nextActionDuration = nextActionDuration;
        this.stepPower = stepPower;
        time = 0;
        this.rb = rb;
        this.wasStep = false;
        enemyController.SetIsInvincible(true);
    }

    public void DoDemonStates()
    {
        time += Time.deltaTime;
        //Debug.Log($"time: {time}");
        switch (state)
        {
            case DemonState.Chasing: DoChasing(); break;
            case DemonState.Step:   TakeStep(stepDir); break;
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

        //Debug.Log("SetDestination");
        enemyController.SetDestination(enemyController.Target.Value, enemyController.Speed);
    }

    private void DoAttack()
    {
        //Debug.Log($"IsCooldownReady: {enemyController.IsCooldownReady}");
        if (!enemyController.IsCooldownReady)  return;

        Debug.Log("Demon.DoAttack");
        enemyController.Stop();
        if (enemyController.TryAttack())
        {
            float lotteryTakeStep = UnityEngine.Random.Range(0, 100);
            Debug.Log($"LTS: {lotteryTakeStep}");
            if (lotteryTakeStep < probabilityOfTakeStep)
            {
                SetState(DemonState.Step);
                stepDir = (StepDirection)Enum.ToObject(typeof(StepDirection), UnityEngine.Random.Range(0, (int)StepDirection.Max));
                Debug.Log($"stepDir: {stepDir}");
                TakeStep(stepDir);
                //TakeStep(StepDirection.Back);
                return;
            }
        }

        if (DistanceToTarget > enemyController.AttackRange)
        {
            SetState(DemonState.Chasing);
        }
    }



    private void TakeStep(StepDirection stepDir)
    {
        //Debug.Log("TakeStep");
        if (time >= 1f)
        {
            rb.linearVelocity = Vector3.zero;

            if (time < 1.3f) return;
            SetState(DemonState.Chasing);
            wasStep = false;
            return;
        }

        if (wasStep) return;
        Vector3 stepForce = Vector3.zero;
        switch (stepDir)
        {
            case StepDirection.Back: stepForce = -Vector3.forward * stepPower; break;
            case StepDirection.Left: stepForce = -Vector3.right * stepPower; break;
            case StepDirection.Right: stepForce = Vector3.right * stepPower; break;
        }
        //Debug.Log($"stepForce: {stepForce}");
        
        rb.AddForce(stepForce, mode: ForceMode.Acceleration);
        wasStep = true;
    }

    private void SetState(DemonState state)
    {
        this.state = state;
        time = 0;
    }
}
