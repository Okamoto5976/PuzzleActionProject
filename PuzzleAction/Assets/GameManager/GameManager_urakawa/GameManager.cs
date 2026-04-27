using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private IntRunTime m_scoreRuntime;
    [SerializeField] private IntRunTime m_moneyRuntime;
    [SerializeField] private TimeManager timemanager;

    [Header("Event")]
    [SerializeField] private BoolEventSO m_gameOverUIEvent;
    [SerializeField] private BoolEventSO m_menuUIEvent;
    [SerializeField] private BoolEventSO m_optionUIEvent;

    private bool m_isGameOver = false;
    
    void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log("何かキー押された");
            GameOver();
        }
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
    

    //プレイヤー死亡を受け取る
    public void OnPlayerDead()
    {
        GameOver();
    }

    //ゲームオーバー
    public void GameOver()
    {
        if (m_isGameOver) return;

        m_isGameOver = true;

        Debug.Log("ゲームオーバー");

        Time.timeScale = 0f;

        m_gameOverUIEvent.Raise(true);
    }

}