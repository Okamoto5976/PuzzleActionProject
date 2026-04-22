using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private IntRunTime m_scoreRuntime;
    [SerializeField] private IntRunTime m_moneyRuntime;
    [SerializeField] private TimeManager timemanager;

    private bool m_isGameOver = false;
    
    void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        //ゲームオーバー後に止める
        if (m_isGameOver) return;

        timemanager.DecreaseValue(Time.deltaTime);

        //デバック用
        Debug.Log($"Score: {m_scoreRuntime.Value} | Money: {m_moneyRuntime.Value} | Time: {timemanager.Value:F1}");
        
        //時間切れ
        if (timemanager.Value <= 0)
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