using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    [SerializeField] private int m_level;
    [SerializeField] private int m_money;

    //property
    public int Level => m_level;
    public int Money => m_money;

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

}
