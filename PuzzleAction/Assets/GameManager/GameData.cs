using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game/Data")]
public class GameData : ScriptableObject
{
    [SerializeField] private int startScore = 0; //スコア変更
    [SerializeField] private int startMoney = 0; //お金変更
    [SerializeField] private float timeLimit = 60f; //時間変更

    public int StartScore => startScore;
    public int StartMoney => startMoney;
    public float TimeLimit => timeLimit;
}
