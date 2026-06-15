using UnityEngine;

public class TestTeam : MonoBehaviour
{
    public enum TeamType
    {
        Player,
        Enemy,
        Nature
    }

    [SerializeField]
    private TeamType m_team;

    public TeamType Team => m_team;

    public virtual bool CanHit(
        TestTeam other
        )
    {
        if ( other == null )
        {
            return false;
        }

        if (
            Team == TeamType.Nature &&
            other.Team == TeamType.Nature
          )
        {
            return true;
        }

        if(Team==other.Team)
        {
            return false;
        }

        return true;
    }
}