using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData data;
    [SerializeField] private GameRuntimeData runtime;

    private bool isGameOver = false;

    void Start()
    {
        // 初期化
        runtime.SetScore(data.StartScore);
        runtime.SetMoney(data.StartMoney);
        runtime.SetTime(data.TimeLimit);

        Time.timeScale = 1f;
    }

    void Update()
    {
        //ゲームオーバー後に止める
        if (isGameOver) return;

        runtime.SetTime(runtime.Time - Time.deltaTime);

        //時間切れ
        if (runtime.Time <= 0)
        {
            runtime.SetTime(0);
            GameOver();
        }

        Debug.Log($"Score: {runtime.Score} | Money: {runtime.Money} | Time: {runtime.Time:F1}");
    }

    //ゲームオーバー
    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("ゲームオーバー");
        Time.timeScale = 0f;
    }
    //スコア
    public void AddScore(int value)
    {
        runtime.SetScore(runtime.Score + value);
    }

    //お金
    public void AddMoney(int value)
    {
        runtime.SetMoney(runtime.Money + value);
    }
}