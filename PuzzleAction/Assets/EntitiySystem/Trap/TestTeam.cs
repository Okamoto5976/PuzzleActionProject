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
}
