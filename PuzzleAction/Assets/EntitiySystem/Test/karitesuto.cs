using UnityEngine;

public enum TrapTeam
{
    Player,
    Enemy,
    Nature//さん
}

public class karitesuto : MonoBehaviour
{
    [Header("このオブジェクトチーム")]
    public TrapTeam m_MyTeam;
}
