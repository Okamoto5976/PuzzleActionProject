using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData data;
    [SerializeField] private GameRuntimeData runtime;

    private bool isGameOver=false;//New

    void Start()
    {
        //初期化
        runtime.SetScore(data.StartScore);
        runtime.SetMoney(data.StartMoney);
        runtime.SetTime(data.TimeLimit);

        //リセット★
        Time.timeScale = 1f;

    }

    void Update()
    {
        //ゲームオーバー後は止める★
        if (isGameOver) return;

        runtime.SetTime(runtime.Time - Time.deltaTime);
        
        if(runtime.Time <= 0)
        {
            runtime.SetTime(0);
            GameOver();
        }

        Debug.Log($"Score:{runtime.Score}|Money:{runtime.Money}|Time:{runtime.Time:F1}");
    }

    //ゲームオーバー★
    private void GameOver()
    {
        isGameOver = true;
        Debug.Log("ゲームオーバー");
        Time.timeScale = 0f;
    }

    //スコア
    private void AddScore(int value)
    {
        runtime.SetScore(runtime.Score + value);
    }

    //お金
    private void AddMoney(int value)
    {
        runtime.SetMoney(runtime.Money + value);
    }
}
