using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData data;

    [SerializeField] private ScoreRuntime scoreRuntime;
    [SerializeField] private MoneyRuntime moneyRuntime;
    [SerializeField] private TimeRuntime timeRunTime;

    private bool m_isGameOver = false;
    
    void Start()
    {
        scoreRuntime.SetValue(data.StartScore);    
        moneyRuntime.SetValue(data.StartMoney);
        timeRunTime.SetValue(data.TimeLimit);
   
        //デバック用
        Debug.Log("GameManagerが動く");
        Time.timeScale = 1f;
    }

    void Update()
    {
        //ゲームオーバー後に止める
        if (m_isGameOver) return;

        timeRunTime.DecreaseValue(Time.deltaTime);

        //デバック用
        Debug.Log($"Score: {scoreRuntime.Value} | Money: {moneyRuntime.Value} | Time: {timeRunTime.Value:F1}");
        
        //時間切れ
        if (timeRunTime.Value <= 0)
        {
            GameOver();
        }
    }
    
    //ゲームオーバー
    public void GameOver()
    {
        m_isGameOver = true;

        //デバック用
        Debug.Log("ゲームオーバー");
        Time.timeScale = 0f;
    }
 }