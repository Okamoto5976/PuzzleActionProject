using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData data;
    [SerializeField] private GameRuntimeData runtime;

    private bool m_isGameOver = false;
    
    void Start()
    {
        runtime.Init(data.StartScore, data.StartMoney, data.TimeLimit);
        
        //デバック用
        Debug.Log("GameManagerが動く");
        Time.timeScale = 1f;
    }

    void Update()
    {
        //ゲームオーバー後に止める
        if (m_isGameOver) return;

        runtime.DecreaseTime(Time.deltaTime);

        //デバック用
        //Debug.Log($"Score: {runtime.Score} | Money: {runtime.Money} | Time: {runtime.Time:F1}");

        //時間切れ★
        if (runtime.Time <= 0)
        {
            GameOver();
        }
    }

    //ゲームオーバー★
    public void GameOver()
    {
        m_isGameOver = true;

        //デバック用
        Debug.Log("ゲームオーバー");
        Time.timeScale = 0f;
    }
    //スコア加算★
    public void AddScore(int m_value)
    {
        runtime.AddScore(m_value);
    }

    //お金加算★
    public void AddMoney(int m_value)
    {
        runtime.AddMoney(m_value);
    }
}