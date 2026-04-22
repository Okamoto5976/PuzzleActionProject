using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    //内部データ
    [Header("HP Setting")]
    [SerializeField] private int maxHP = 100; //仮置き
    private int m_currentHP;

    [Header("Economy")]
    private int m_money;

    [Header("Score")]
    private int m_score;
    
    //初期化
    private void Start()
    {
        m_currentHP=maxHP;
        m_money = 0;
        m_score = 0;
    }

    //HP管理ロジック
    public void TakeDamage(int damage)
    {
        if(damage <= 0)return;
        m_currentHP = Mathf.Max(0, m_currentHP - damage);
    }

    //回復
    public void Heal(int amount)
    {
        if (amount <= 0) return;
        m_currentHP = Mathf.Min(maxHP, m_currentHP + amount);
    }


    //お金管理ロジック
    public void AddMoney(int amount)
    {
        if (amount <= 0) return;
        m_money += amount;
    }

    public bool TrySpendMoney(int amount)
    {
        if(amount <=0)return false;
        if(m_money>=amount)
        {
            m_money -= amount;
            return true; //支払い成功
        }      
        return false; //お金不足
    }

    //スコア管理ロジック
    public void AddScore(int points)
    {
        if (points <= 0) return;
        m_score += points;    
    }
    
    //外部から呼び出す
    public int CureentHP => m_currentHP;
    public int CureentMoney => m_money;
    public int CureentScore => m_score;
}
