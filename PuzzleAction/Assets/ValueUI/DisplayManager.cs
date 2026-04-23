using TMPro;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    // ここが「None」の枠を作る部分です！
    [Header("UI References")]
    [SerializeField] private HPUI hpUI;      // HPUIスクリプトを紐付ける枠
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private Mniy moneyUI;   // Mniyスクリプトを紐付ける枠
    [SerializeField] private sufor scoreUI;  // suforスクリプトを紐付ける枠

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
        
        // 起動時に今の値をUIに送る
        UpdateAllUI();
    }

    // HPを減らす
    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;
        m_currentHP = Mathf.Max(0, m_currentHP - damage);
        
        // HPUIに「表示を更新して！」と命令する
        if (hpUI != null) hpUI.UpdateHPBar(m_currentHP, maxHP);
    }

    // お金を増やす
    public void AddMoney(int amount)
    {
        if (amount <= 0) return;
        m_money += amount;
        
        // Mniyに「表示を更新して！」と命令する
        if (moneyUI != null) moneyUI.UpdateMoneyDisplay(m_money);
    }

    // スコアを増やす
    public void AddScore(int points)
    {
        if (points <= 0) return;
        m_score += points;
        
        // suforに「表示を更新して！」と命令する
        if (scoreUI != null) scoreUI.UpdateScoreDisplay(m_score);
    }

    private void UpdateAllUI()
    {
        if (hpUI != null) hpUI.UpdateHPBar(m_currentHP, maxHP);
        if (moneyUI != null) moneyUI.UpdateMoneyDisplay(m_money);
        if (scoreUI != null) scoreUI.UpdateScoreDisplay(m_score);
    }
}
