using UnityEngine;

[CreateAssetMenu(fileName = "GameRuntimeData", menuName = "Game/RuntimeData")]
public class GameRuntimeData : ScriptableObject
{
    [SerializeField] private int score; //スコア
    [SerializeField] private int money; //お金
    [SerializeField] private float time;  //時間

    public int Score => score;
    public int Money => money;
    public float Time => time;

    //値変更
    public void SetScore(int value) =>score = value;
    public void SetMoney(int value) =>money = value;
    public void SetTime(float value) => time = value;
}
