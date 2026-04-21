using UnityEngine;

public class EnemyHP : EntityHP, ITeam
{
    private TeamNames m_team;
    public TeamNames Team => m_team;

    protected override void Die()
    {
        Debug.Log("Enemy has died");
    }
}
