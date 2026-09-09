using UnityEngine;

[System.Serializable]
public class EnemyWormController
{
    [SerializeField] private float detectDistance;
    [SerializeField] private float attackAnimationTime;
    [SerializeField] private float attackAnimationCooldown;
    [SerializeField] private float randomRange;
    private Transform transform;
    private float time;
    private EnemyController enemyController;
    private bool hasAttacked;
    private enum WormState
    {
        Relocate,
        Standby,
        Trigger
    }

    private WormState state;

    public void Initialize(EnemyController enemyController, Transform transform)
    {
        state = WormState.Standby;
        this.enemyController = enemyController;
        this.transform = transform;
    }

    public void DoWormState()
    {
        time += Time.deltaTime;
        switch (state)
        {
            case WormState.Relocate: DoRelocate(); break;
            case WormState.Standby: DoStandby(); break;
            case WormState.Trigger: DoTrigger(); break;
        }
    }

    private void DoRelocate()
    {
        var position = enemyController.GetRandomPosition(randomRange);
        enemyController.TeleportToPosition(position);
        ChangeState(WormState.Standby);
    }

    private void DoStandby()
    {
        if (enemyController.Target == null) return;

        var targetPosition = enemyController.Target.Value;
        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance <= detectDistance)
        {
            ChangeState(WormState.Trigger);
            hasAttacked = false;
        }
    }

    private void DoTrigger()
    {   
        if (time >= attackAnimationTime && !hasAttacked)
        {
            enemyController.Attack();
            hasAttacked = true;
        }
        if (time >= attackAnimationTime + attackAnimationCooldown)
        {
            ChangeState(WormState.Relocate);
        }
    }

    private void ChangeState(WormState state)
    {
        this.state = state;
        time = 0;
    }
}
