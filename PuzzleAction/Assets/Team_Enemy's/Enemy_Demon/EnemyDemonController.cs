using System;
using UnityEngine;

public class EnemyDemonController : MonoBehaviour
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

    private float time;

    private float stepCooldown;
    private float probabilityOfTakeStep;
    private float nextActionDuration;


    private float DistanceToTarget => Vector3.Distance(transform.position, enemyController.Target.Value);


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

    }

    private void DoAttack()
    {

    }



    private void TakeStep(StepDirection stepDir)
    {

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
