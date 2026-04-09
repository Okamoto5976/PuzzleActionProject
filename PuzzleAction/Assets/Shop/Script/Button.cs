using UnityEngine;
using TMPro;
using System.Collections;

public class Button : MonoBehaviour
{
    [SerializeField] private int m_price = 100;  //このアイテムの値段
    [SerializeField] private Money m_money;   //お金管理
    [SerializeField] private UnityEngine.UI.Button m_button;  //このボタン自身
    [SerializeField] private TextMeshProUGUI m_messageText; //メッセージ表示用
    [SerializeField] private Vector3 m_fadeOutOffset;



void Start()
    {
        //ボタンが押された時の処理を登録
        m_button.onClick.AddListener(BuyItem);
    }
    public void BuyItem()
    {
        if (m_money.UseMoney(m_price))
        {
            ShowMessage("Buy \n : " + m_price, Color.blue);            //ここにアイテム入手、効果発動などを書く
        }
        else
        {
            ShowMessage("No Money" , Color.blue);
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
