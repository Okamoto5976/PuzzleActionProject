using System.Runtime.CompilerServices;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private MessageManager m_messageManager;
    [SerializeField] private float waitTime = 5f;  //何秒で発動
    private float timer = 0f;
    private bool isAction = false;
    private bool isFirst = true;
    void Start()
    {
     m_messageManager.ShowByState(State.Welcome);
        timer = 0f;
    }
    //ボタンなどの操作があったらリセット
    void Update()
    {
        if (isAction)
        {
            timer = 0f;
            isAction = false;
            return;
        }
        timer += Time.deltaTime;
        if (timer >= waitTime)
        {
            timer = 0f;
            ShowRandomTalk();
        }
    }
    public void NotifyAction()
    {
        isAction = true;
    }
    void ShowRandomTalk()
    {
        m_messageManager.ShowRandomNoneMessage();
    }
}