using UnityEngine;

[CreateAssetMenu(fileName = "GameRuntimeData", menuName = "Game/RuntimeData")]
public class GameRuntimeData : ScriptableObject
{
    [SerializeField] private int score;
    [SerializeField] private int money;
    [SerializeField] private float time;

    public int Score => score; //スコア
    public int Money => money; //お金
    public float Time => time; //時間

    //初期化
    public void Init(int m_startScore,int m_startMoney,float m_startTime)
    {
        score = m_startScore;
        money = m_startMoney;
        time = m_startTime;
    }

    //スコア加算計算
    public void AddScore(int m_addValue)
    {
        score += m_addValue;
    }

    //お金換算計算
    public void AddMoney(int m_addValue)
    {
        money += m_addValue;
    }

    //時間減少計算
    public void DecreaseTime(float m_deltaTime)
    {
        time -= m_deltaTime;
        if (time < 0) time = 0;
    }
}