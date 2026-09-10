using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        //m_level = 1;
    }

    [SerializeField] private int m_level;
    [SerializeField] private int m_money;

    private bool m_hasKey = false;
    //property
    public int Level => m_level;
    public int Money => m_money;
    public bool HasKey => m_hasKey;

    public void AddLevel(int value)
    {
        if (value < 0)
        {
            Debug.LogError("get value is negative value");
            return;
        }

        m_level += value;
    }

    public void SetLevel(int value)
    {
        m_level = value;
    }

    //+ or - 
    public bool ModifyMoney(int value)
    {
        if(0 > m_money + value)
        {
            return false;
        }

        m_money += value;
        return true;
    }

    public void SetMoney(int value)
    {
        m_money = value;
    }

    public void OnStopTime(bool isStop)
    {
        if(isStop)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    //if kill boss
    public void SetKey()
    {
        m_hasKey = true;
    }

    //Create Map before
    public void ResetData()
    {
        m_hasKey = false;
    }
}
