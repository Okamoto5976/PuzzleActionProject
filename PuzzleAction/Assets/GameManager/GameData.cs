using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game/Data")]
public class GameData : ScriptableObject
{
    [SerializeField] private int m_startScore = 0;　　//スコア
    [SerializeField] private int m_startMoney = 0;    //お金
    [SerializeField] private float m_timeLimit = 60f; //時間

    public int StartScore => m_startScore;
    public int StartMoney => m_startMoney;
    public float TimeLimit => m_timeLimit;
}