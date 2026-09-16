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
        Step,
        Max
    }

    private EnemyController enemyController;
    private DemonState state;
    private StepDirection stepDir;
    private ChasingAction action;
    private Transform transform;
    private Rigidbody rb;

    private float time;
    private float untilNextAction;   // Time until the next action.

    private float probabilityOfTakeStep;
    private float nextActionDuration;
    private float nADMin;   //  nextActionDuration(Min)
    private float nADMax;   //  nextActionDuration(Max)
    private float stepPower;
    private float meanderingRange;

    private bool wasStep;
    private bool moveRight;


    private float DistanceToTarget => Vector3.Distance(transform.position, enemyController.Target.Value);

    public void Initialize(EnemyController enemyController, Transform transform, float probablityOfTakeStep, float nADMin, float nADMax,
        float stepPower, Rigidbody rb, float meanderingRange)
    {
        this.enemyController = enemyController;
        this.transform = transform;
        this.probabilityOfTakeStep = probablityOfTakeStep;
        this.nextActionDuration = nADMin;
        this.nADMin = nADMin;
        this.nADMax = nADMax;
        this.stepPower = stepPower;
        this.meanderingRange = meanderingRange;
        time = 0;
        untilNextAction = 0;
        action = ChasingAction.None;
        this.rb = rb;
        this.wasStep = false;
        this.moveRight = false;
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

        // Action
        untilNextAction += Time.deltaTime;
        if (untilNextAction >= nextActionDuration)
        {
            //action = (ChasingAction)Enum.ToObject(typeof(ChasingAction), UnityEngine.Random.Range(0, (int)ChasingAction.Max));
            action = ChasingAction.Meandering;
            nextActionDuration = UnityEngine.Random.Range(nADMin, nADMax);
            untilNextAction = 0f;
            Debug.Log($"nextDuration: {nextActionDuration}");
            Debug.Log($"action: {action}");
        }
        TakeAction(action);

        //Debug.Log("SetDestination");
        enemyController.SetDestination(enemyController.Target.Value, enemyController.Speed);
    }

    private void DoAttack()
    {
        if (!enemyController.IsCooldownReady)  return;

        Debug.Log("Demon.DoAttack");
        enemyController.Stop();
        if (enemyController.TryAttack())
        {
            float lotteryTakeStep = UnityEngine.Random.Range(0, 100);
            //Debug.Log($"LTS: {lotteryTakeStep}");
            if (lotteryTakeStep < probabilityOfTakeStep)
            {
                SetState(DemonState.Step);
                stepDir = (StepDirection)Enum.ToObject(typeof(StepDirection), UnityEngine.Random.Range(0, (int)StepDirection.Max));
                //Debug.Log($"stepDir: {stepDir}");
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
            case StepDirection.Back: stepForce = -transform.forward * stepPower; break;
            case StepDirection.Left: stepForce = -transform.right * stepPower; break;
            case StepDirection.Right: stepForce = transform.right * stepPower; break;
        }
        //Debug.Log($"stepForce: {stepForce}");
        
        rb.AddForce(stepForce, mode: ForceMode.Acceleration);
        wasStep = true;
    }

    private void TakeAction(ChasingAction action)
    {
        if (action == ChasingAction.None) return;

        switch (action)
        {
            case ChasingAction.Meandering:
                {
                    Vector3 force = rb.GetAccumulatedForce();
                    float hForce = force.x * force.x + force.z * force.z;   // horizontal force
                    Debug.Log($"force: {force}");
                    Debug.Log($"hForce: {hForce}");

                    if (hForce < 4 && moveRight)
                        rb.AddForce(transform.right * meanderingRange * Time.deltaTime);
                    else if (hForce < 4 && !moveRight)
                        rb.AddForce(-transform.right * meanderingRange * Time.deltaTime);

                    if (hForce >= 4) moveRight = !moveRight;
                }
                break;

            case ChasingAction.Zigzag:
                {

                }
                break;

            case ChasingAction.Stop:
                {

                }
                break;

            case ChasingAction.Step:
                {

                }
                break;
        }
    }

    private void SetState(DemonState state)
    {
        this.state = state;
        time = 0;
    }
}
