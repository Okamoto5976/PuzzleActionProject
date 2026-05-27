using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using Unity.VisualScripting;
using System.Collections.Generic;

public class MessageManager : MonoBehaviour
{
    [SerializeField] private MessageSO[] m_messages;
    [SerializeField] private TextMeshProUGUI m_messageText;
    [SerializeField] private TextMeshProUGUI m_speakerText;
    [SerializeField] private GameObject m_speechParent ;
    [SerializeField] private UnityEngine.UI.Button m_button;
    [SerializeField] private float m_idleTime = 5f;
    private float m_timer = 0f;
    private int m_index = 0;
    private bool m_isActive = false; //購入、入店、退店
                                     // private bool m_isAction = false;
    private bool m_isIdleAllowed = true;
    private float m_idleDelayAfterAction = 2f;
    private float m_idleCooldown = 0f;

    private MessageType m_messageType;
    private MessageSO GetMessageByType(MessageType type)
    {
        foreach (var data in m_messages)
        {
            if (data.m_messageType == type)
            {
                return data;
            }
        }
        return null;
    }

    void Start()
    {
        m_speechParent.SetActive(true);
        // m_button.onClick.AddListener(ShowCloseMessage);
        m_button.onClick.AddListener(OnClickShopExit);
    }

   void Update()
    {
        if (!m_isActive) return;
        //Idle停止中の処理
        if (!m_isIdleAllowed) //return;
        {
            m_idleCooldown += Time.deltaTime;
            if (m_idleCooldown >= m_idleDelayAfterAction)
            {
                m_isIdleAllowed = true;
                m_idleCooldown = 0f;
            }
            return;  //ここで止める
        }

        m_timer += Time.deltaTime;
        if (m_timer >= m_idleTime)
        {
            ShowRandomNoneMessage();
            ResetTimer();
        }
    }

    public void ResetTimer()
    {
        m_timer = 0f;
    }

    public void OnAction()
    {
       // m_isActive = true;
       ResetTimer();
    }

    void EndMessage()
    {
        m_isActive = false;
        m_speechParent.SetActive(false);
        m_messageText.text = "";
        if (m_speakerText != null)
            m_speakerText.text = "";
    }
    //通常会話再生（Noneのみ）
    public void StartSequence()
    {
        m_index = 0;
        m_isActive = true;
        ResetTimer();
        m_speechParent.SetActive(true);
        //ShowNextNoneMessage();
    }

    public void ShowBuyMessage(string itemName, int count)
    {
        m_isIdleAllowed = false;
        //吹き出しを表示
        m_speechParent.SetActive(true);
        //メッセージ作成
        m_messageText.text = itemName + "wo" + count + "Buy";
        if (m_speakerText != null)
        {
            m_speakerText.text = "Shop";
            ResetTimer();
        }
    }
    public void ShowRandomNoneMessage()
    {
        if (m_speechParent == null) return;
        var list = new List<MessageSO>();
        //System.Collections.Generic.List<MessageSO>();
        foreach (var data in m_messages)
        {
            if (data.m_messageType == MessageType.Normal)
            {
                list.Add(data);
            }
        }
        if (list.Count == 0) return;

        int rand = Random.Range(0, list.Count);
        m_speechParent.SetActive(true);
        Show(list[rand]);
    }
    public void ShowCloseMessage()
     {

        //m_speechParent.SetActive(true);
        while (m_index < m_messages.Length)
        {
            MessageSO data = m_messages[m_index];
            m_index++;

        }
    }
    public void ShowMessageByType(MessageType type)
    {
        if (m_speechParent == null)
        {
            Debug.Log("sppechParentがnull");
            return;
        }

        MessageSO data = null;

        switch (type)
        {
            case MessageType.ShopEnter: data = GetMessageByType(MessageType.ShopEnter); break;
            case MessageType.Normal: ShowRandomNoneMessage();
                return;

            case MessageType.Buy:data = GetMessageByType(MessageType.Buy);
                break;


            case MessageType.NoMoney: data = GetMessageByType(MessageType.NoMoney);
                break;

            case MessageType.InventoryFull: data = GetMessageByType(MessageType.InventoryFull);
                break;

            case MessageType.ShopExit : data = GetMessageByType(MessageType.ShopExit);
                break;

        }
        if (data == null)
        {
            Debug.Log("メッセージが見つかりません" + type);
            return;
        }
        m_speechParent.SetActive(true);
        Show(data);
        ResetTimer();
    }


    void Show(MessageSO data)
    {
        m_isIdleAllowed = false;
        m_messageText.text = data.message;

        if (m_speakerText != null)
            m_speakerText.text = data.speaker;
    }
    public void OnClickShopExit()
    {
        ShowMessageByType(MessageType.ShopExit);
    }

}