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
    public void Init(int startScore,int startMoney,float startTime)
    {
        score = startScore;
        money = startMoney;
        time = startTime;
    }

    // 値変更用
    public void SetScore(int value) => score = value;
    public void SetMoney(int value) => money = value;
    public void SetTime(float value) => time = value;
}