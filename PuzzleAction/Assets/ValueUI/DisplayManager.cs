using TMPro;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private HPUI hpUI;      
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private Mniy moneyUI;  
    [SerializeField] private sufor scoreUI;  

    [Header("HP Setting")]
    [SerializeField] private int maxHP = 100;
    private int m_currentHP;

    [Header("Money")]
    private int m_money;

    [Header("Score")]
    private int m_score;

    private void Start()
    {
        m_currentHP = maxHP;
        m_money = 0;
        m_score = 0;
        
        UpdateAllUI();
    }

    // HP‚ðŒ¸‚ç‚·
    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        m_currentHP = Mathf.Max(0, m_currentHP - damage);
       
        if (hpUI != null) hpUI.UpdateHPBar(m_currentHP, maxHP);
    }

    // ‚¨‹à‚ð‘‚â‚·
    public void AddMoney(int amount)
    {
        if (amount <= 0) return;
        m_money += amount;
        
        if (moneyUI != null) moneyUI.UpdateMoneyDisplay(m_money);
    }

    // ƒXƒRƒA‚ð‘‚â‚·
    public void AddScore(int points)
    {
        if (points <= 0) return;
        m_score += points;
       
        if (scoreUI != null) scoreUI.UpdateScoreDisplay(m_score);
    }

    private void UpdateAllUI()
    {
        if (hpUI != null) hpUI.UpdateHPBar(m_currentHP, maxHP);
        if (moneyUI != null) moneyUI.UpdateMoneyDisplay(m_money);
        if (scoreUI != null) scoreUI.UpdateScoreDisplay(m_score);
    }
}
