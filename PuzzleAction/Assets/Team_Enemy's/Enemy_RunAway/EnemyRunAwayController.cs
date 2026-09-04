using UnityEngine;

public class EnemyRunAwayController
{
    private EnemyController enemyController;
    private Transform transform;
    private float runRange;

    private float DistanceToTarget => Vector3.Distance(transform.position, enemyController.Target.Value);

    private enum RunAwayState
    {
        Idle,
        Running
    }

    private RunAwayState state;

    public void Initialize(EnemyController enemyController, Transform transform, float runRange)
    {
        this.enemyController = enemyController;
        this.transform = transform;
        this.runRange = runRange;
        state = RunAwayState.Idle;
    }

    public void DoRunAwayStates()
    {
        switch (state)
        {
            case RunAwayState.Idle: DoIdle(); break;
            case RunAwayState.Running: DoRunning(); break;
        }

        if (enemyController.TryUseCooldown())
        {
            enemyController.UseItem(enemyController.Target.Value - transform.position);
        }
    }

    private void DoIdle()
    {
        if (enemyController.Target == null) return;
        if (DistanceToTarget <= runRange)
        {
            ChangeState(RunAwayState.Running);
            return;
        }

        enemyController.Stop();
    }

    private void DoRunning()
    {
        if (enemyController.Target == null) return;
        if (DistanceToTarget > runRange)
        {
            ChangeState(RunAwayState.Idle);
            return;
        }

        enemyController.SetDestination(transform.position - enemyController.Target.Value, enemyController.Speed);
    }

    private void ChangeState(RunAwayState state)
    {
        this.state = state;
    }
}
