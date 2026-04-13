using UnityEngine;
using TMPro;
using System.Collections;
using JetBrains.Annotations;

public class Button : MonoBehaviour
{
    [SerializeField] private int m_price = 100;  //このアイテムの値段
    [SerializeField] private Money m_money;   //お金管理
    [SerializeField] private UnityEngine.UI.Button m_button;  //このボタン自身
    [SerializeField] private TextMeshProUGUI m_messageText; //メッセージ表示用
    [SerializeField] private Vector3 m_fadeOutOffset;
    [SerializeField] private string m_itemName;    //送るアイテム名
    [SerializeField] private Inventory m_inventory;
    [SerializeField] private int m_itemId;  //このボタンが売るアイテム

    public string ItemName => m_itemName;
    public int ItemId => m_itemId;
    public int Price => m_price;

void Start()
    {
        //ボタンが押された時の処理を登録
        //m_button.onClick.AddListener(BuyItem);
       // m_button.onClick.AddListener(SendItemAtInventory);
        //m_button.onClick.AddListener(～～) アイテムをインベントリに送る関数を記入
    }
    public void BuyItem()
    {
        if (m_money.UseMoney(m_price))
        {
            ShowMessage("Buy \n : " + m_price, Color.blue);            //ここにアイテム入手、効果発動などを書く
            //お金が足りた時だけインベントリに送る
            SendItemAtInventory();
        }
        else
        {
            ShowMessage("No Money" , Color.blue);
        }
    }

    public void SendItemAtInventory()
    {
        if (m_inventory.AddItem(m_itemId))
        {
            Debug.Log("インベントリにアイテムを送った" +  m_itemName);
        }
        else
        {
            Debug.Log("インベントリがいっぱいで送れない");
        }
    }
    void ShowMessage(string message, Color color)
    {
        StopAllCoroutines();        //前のフェードを止める

        m_messageText.text = message;
        m_messageText.color = new Color(color.r,color.g, color.b, 1f);
        m_messageText.transform.position = m_button.transform.position + m_fadeOutOffset;

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        //表示時間
        yield return new WaitForSeconds(1.5f);

        float fadeTime = 1f;
        float elapsed = 0f;
        Color c = m_messageText.color;

        while ((elapsed < fadeTime))
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeTime);
            m_messageText.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        m_messageText.text = ""; 
    }
}
