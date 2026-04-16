using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData data;
    [SerializeField] private GameRuntimeData runtime;

    private bool isGameOver = false;

    void Start()
    {
        runtime.Init(data.StartScore, data.StartMoney, data.TimeLimit);
     
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
        //デバック
        //Debug.Log($"Score: {runtime.Score} | Money: {runtime.Money} | Time: {runtime.Time:F1}");
    }

    //ゲームオーバー
    public void GameOver()
    {
        isGameOver = true;
        //Debug.Log("ゲームオーバー");
        Time.timeScale = 0f;
    }
    //スコア加算
    public void AddScore(int m_value)
    {
        runtime.AddScore(m_value);
    }

    //お金加算　
    public void AddMoney(int m_value)
    {
        runtime.AddMoney(m_value);
    }
}