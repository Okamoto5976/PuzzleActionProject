using UnityEngine;

public class Test_chara : MonoBehaviour, IDamage, ITeam
{
    public void TakeDamage(DamageData data, DamageResult result)
    {
        Debug.Log("TakeDamage");
    }
    [SerializeField] private TeamType m_team;
    public TeamType Team { get; set; }
    public AttackHitBox attackHitBox;

    void Awake()
    {
        Team = m_team;
    }
}
