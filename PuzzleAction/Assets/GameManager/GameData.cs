using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game/Data")]
public class GameData : ScriptableObject
{
    [SerializeField] private int startScore = 0;　　//スコア
    [SerializeField] private int startMoney = 0;    //お金
    [SerializeField] private float timeLimit = 60f; //時間

    public int StartScore => startScore;
    public int StartMoney => startMoney;
    public float TimeLimit => timeLimit;
}