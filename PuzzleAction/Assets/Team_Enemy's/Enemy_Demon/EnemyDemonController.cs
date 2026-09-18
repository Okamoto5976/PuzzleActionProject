using System;
using UnityEngine;

public class EnemyDemonController
{
    private enum DemonState
    {
        Chasing,
        Step,
        Attack,
        Stop
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

    //private float time;
    private float untilNextAction;   // Time until the next action.
    private float time_zigzag;
    private float time_stop;
    private float time_step;

    private float probabilityOfTakeStep;
    private float nextActionDuration;
    private float nADMin;   //  nextActionDuration(Min)
    private float nADMax;   //  nextActionDuration(Max)
    private float stepPower;
    private float stepTime;
    private float waitTimeAfterStep;
    private float zigzagRange;
    private float stopTime;

    private bool wasStep;
    private bool moveRight;


    private float DistanceToTarget => Vector3.Distance(transform.position, enemyController.Target.Value);

    public void Initialize(EnemyController enemyController, Transform transform, float probablityOfTakeStep, float nADMin, float nADMax,
        float stepPower, Rigidbody rb, float zigzagRange, float stopTime, float stepTime, float waitTimeAfterStep)
    {
        this.enemyController = enemyController;
        this.transform = transform;
        this.probabilityOfTakeStep = probablityOfTakeStep;
        this.nextActionDuration = nADMin;
        this.nADMin = nADMin;
        this.nADMax = nADMax;
        this.stepPower = stepPower;
        this.stepTime = stepTime;
        this.waitTimeAfterStep = waitTimeAfterStep;
        this.zigzagRange = zigzagRange;
        this.stopTime = stopTime;
        untilNextAction = 0;
        time_zigzag = 0;
        time_stop = 0;
        time_step = 0;
        action = ChasingAction.None;
        this.rb = rb;
        this.wasStep = false;
        this.moveRight = false;
        enemyController.SetIsInvincible(true);
    }

    public void DoDemonStates()
    {
        //time += Time.deltaTime;
        switch (state)
        {
            case DemonState.Chasing: DoChasing(); break;
            case DemonState.Step: TakeStep(stepDir); break;
            case DemonState.Attack: DoAttack(); break;
            case DemonState.Stop:   DoStop(); break;
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

        // Action
        untilNextAction += Time.deltaTime;
        if (untilNextAction >= nextActionDuration)
        {
            action = (ChasingAction)Enum.ToObject(typeof(ChasingAction), UnityEngine.Random.Range(1, (int)ChasingAction.Max));  // None is exclusion.
            //action = ChasingAction.Step;

            untilNextAction = 0f;
            nextActionDuration = UnityEngine.Random.Range(nADMin, nADMax);
            Debug.Log($"nextDuration: {nextActionDuration}");
            Debug.Log($"action: {action}");
        }
        TakeAction(action);
    }

    private void DoAttack()
    {
        if (!enemyController.IsCooldownReady)  return;

        enemyController.Stop();
        rb.linearVelocity = Vector3.zero;
        if (enemyController.TryAttack())
        {
            float lotteryTakeStep = UnityEngine.Random.Range(0, 100);
            //Debug.Log($"LTS: {lotteryTakeStep}");
            if (lotteryTakeStep < probabilityOfTakeStep)
            {
                SetState(DemonState.Step);
                stepDir = (StepDirection)Enum.ToObject(typeof(StepDirection), UnityEngine.Random.Range(0, (int)StepDirection.Max));
                //TakeStep(stepDir);
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
        time_step += Time.deltaTime;
        if (time_step >= stepTime)
        {
            rb.linearVelocity = Vector3.zero;

            if (time_step < stepTime + waitTimeAfterStep) return;
            SetState(DemonState.Chasing);
            time_step = 0f;
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
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(stepForce, mode: ForceMode.Acceleration);
        wasStep = true;
    }

    private void DoStop()
    {
        time_stop += Time.deltaTime;

        if (time_stop >= stopTime)
        {
            time_stop = 0;
            SetState(DemonState.Chasing);
            action = ChasingAction.None;
        }
    }

    private void TakeAction(ChasingAction action)
    {
        if (action == ChasingAction.None)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        switch (action)
        {
            case ChasingAction.Zigzag:
                {
                    time_zigzag += Time.deltaTime;
                    //Vector3 force = rb.GetAccumulatedForce();
                    //float hForce = force.x * force.x + force.z * force.z;

                    if (moveRight)
                        rb.AddForce(transform.right * zigzagRange);
                    else if (!moveRight)
                        rb.AddForce(-transform.right * zigzagRange);

                    if (time_zigzag > 0.25f)
                    {
                        time_zigzag = 0f;
                        moveRight = !moveRight;
                        rb.linearVelocity = Vector3.zero;
                    }
                }
                break;

            case ChasingAction.Stop:
                {
                    SetState(DemonState.Stop);
                    enemyController.Stop();
                }
                break;

            case ChasingAction.Step:
                {
                    SetState(DemonState.Step);
                    this.action = ChasingAction.None;
                    enemyController.Stop();
                    stepDir = (StepDirection)Enum.ToObject(typeof(StepDirection), UnityEngine.Random.Range(1, (int)StepDirection.Max)); // Back step is exclusion.
                    Debug.Log($"[TakeAction]stepDir: {stepDir}");
                }
                break;
        }
    }

    private void SetState(DemonState state)
    {
        this.state = state;
    }
}
