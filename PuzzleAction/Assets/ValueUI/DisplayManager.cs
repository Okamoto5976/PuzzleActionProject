using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DisplayManager : MonoBehaviour
{
    [Header("UI References")]
    //[SerializeField] private HPUI hpUI;
    [SerializeField] private PlayerHPUI playerHPUI;
    //[SerializeField] private TMP_Text hpText;
    [SerializeField] private MoneyUI moneyUI;  
    [SerializeField] private ScoreUI scoreUI;
    [SerializeField] private LevelUI levelUI;

    //[Header("HP Setting")]
    //[SerializeField] private int maxHP = 100;
    //private int m_currentHP;

    [Header("Money")]
    private int m_money;
    [SerializeField] private IntRunTime m_moneyDataSO;

    [Header("Score")]
    private int m_score;

    [Header("Level")]
    private int m_level;
    [SerializeField] private IntRunTime m_levelDataSO;

    [SerializeField] private TMP_Text m_testHotberIndexText;

    private void Start()
    {
        //m_currentHP = maxHP;
        m_level = m_levelDataSO.Value;

        //for (int i = 0; i < m_images.Count; i++)
        //{
        //    m_images[i].material.SetFloat("_Alpha", 0f);
        //}

        if (levelUI != null) levelUI.UpdateScoreDisplay(m_level);

    }

    private void Update()
    {
        m_money = m_moneyDataSO.Value;

        if (moneyUI != null) moneyUI.UpdateMoneyDisplay(m_money);
        //UpdateAllUI();
    }

    [SerializeField] private List<Image> m_images;

    public void SetHotberImage(int index, Sprite image)
    {
        m_images[index].sprite = image;
    }

    public void ResetHotberImage(int index)
    {
        m_images[index].sprite = null;
    }

    public void SetIndex(int index)
    {
        m_testHotberIndexText.text = index.ToString();

        //for(int i = 0;  i < m_images.Count; i++)
        //{
        //    m_images[i].material.SetFloat("_Alpha", 0f);
        //}

        //m_images[index].material.SetFloat("_Alpha", 1f);
    }

    //public void UpdatePlayerHP(int currenHP, int maxHP)
    //{
    //    if (playerHPUI != null)
    //    {
    //        playerHPUI.UpdateHP(currenHP, maxHP);
    //    }
    //}

    // HP‚ðŒ¸‚ç‚·
    //public void TakeDamage(int damage)
    //{
    //    if (damage <= 0) return;
    //    m_currentHP = Mathf.Max(0, m_currentHP - damage);

    //    if (hpUI != null) hpUI.UpdateHPBar(m_currentHP, maxHP);

    //    if (playerHPUI != null)
    //    {
    //        playerHPUI.UpdateHP(m_currentHP, maxHP);
    //    }
    //}

    // ‚¨‹à‚ð‘‚â‚·
    //public void AddMoney(int amount)
    //{
    //    if (amount <= 0) return;
    //    m_money += amount;

    //    if (moneyUI != null) moneyUI.UpdateMoneyDisplay(m_money);
    //}

    // ƒXƒRƒA‚ð‘‚â‚·
    //public void AddScore(int points)
    //{
    //    if (points <= 0) return;
    //    m_level += points;

    //    if (scoreUI != null) scoreUI.UpdateScoreDisplay(m_level);
    //}

    //private void UpdateAllUI()
    //{
    //    //if (hpUI != null) hpUI.UpdateHPBar(m_currentHP, maxHP);
    //    //if (playerHPUI != null) playerHPUI.UpdateHP(m_currentHP, maxHP);
    //    if (moneyUI != null) moneyUI.UpdateMoneyDisplay(m_money);
    //    if (scoreUI != null) scoreUI.UpdateScoreDisplay(m_level);
    //}
}
