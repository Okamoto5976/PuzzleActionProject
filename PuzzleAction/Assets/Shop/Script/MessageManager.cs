using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    [Header("Message SO")]
    [SerializeField] private MessageSO_Shop m_messages;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI m_speakerText;
    [SerializeField] private GameObject m_speechParent;
    [SerializeField] private Button m_closeButton;
    private TextDisplay_02 m_textDisplay;
    private void Awake()
    {
        m_textDisplay = GetComponent<TextDisplay_02>();
    }
    private void Start()
    {
        if (m_speechParent != null)
        {
            m_speechParent.SetActive(false);
        }
        //if (m_closeButton != null)
        //{
        //    m_closeButton.onClick.AddListener(CloseMessage);
        //}
    }

    public void MessageDisplay(Enum_ShopMessageType type)
    {
        MessageList message = GetMessage(type);

        if (message == null)
        {
            Debug.LogWarning($"{type} のメッセージが見つかりません");
            return;
        }
        switch (message.m_messageType)
        {
            case Enum_ShopMessageType.Welcome:
                Debug.Log("入店");
                break;

            case Enum_ShopMessageType.Buy:
                Debug.Log("購入成功");
                break;

            case Enum_ShopMessageType.NoMoney:
                Debug.Log("お金不足");
                break;

            case Enum_ShopMessageType.InventoryFull:
                Debug.Log("インベントリ満タン");
                break;

            case Enum_ShopMessageType.SeeYou:
                Debug.Log("退店");
                break;

            default:
                Debug.Log("通常会話");
                break;
        }

        ShowMessage(message.m_messages);
    }
    public void MessageDisplayRandom(Enum_ShopMessageType type)
    {
        List<MessageList> candidates = new();

        foreach (MessageList message in m_messages.m_messageList)
        {
            if (message.m_messageType == type)
            {
                candidates.Add(message);
            }
        }
        if (candidates.Count == 0)
        {
            Debug.LogWarning($"{type} のメッセージがありません");
            return;
        }
        MessageList randomMessage = candidates[Random.Range(0, candidates.Count)];
        ShowMessage(randomMessage.m_messages);
    }

    /// <summary>
    /// 購入専用メッセージ
    /// </summary>
    public void ShowBuyMessage(string itemName, int count)
    {
        string message = $"{itemName}を{count}個購入しました。";

        ShowMessage(message);
    }

    /// <summary>
    /// 共通メッセージ表示処理
    /// </summary>
    private void ShowMessage(string message)
    {
        if (m_speechParent != null)
        {
            m_speechParent.SetActive(true);
        }

        if (m_speakerText != null)
        {
            m_speakerText.text = "Shop";
        }

        if (m_textDisplay != null)
        {
            m_textDisplay.ShowMessageGradually(message);
        }
    }

    /// <summary>
    /// メッセージを閉じる
    /// </summary>
    public void CloseMessage()
    {
        if (m_speechParent != null)
        {
            m_speechParent.SetActive(false);
        }

        if (m_speakerText != null)
        {
            m_speakerText.text = "";
        }

        if (m_textDisplay != null)
        {
            m_textDisplay.ShowMessage("");
        }
    }
    private MessageList GetMessage(Enum_ShopMessageType type)
    {
        foreach (MessageList message in m_messages.m_messageList)
        {
            if (message.m_messageType == type)
            {
                return message;
            }
        }
        return null;
    }

#if UNITY_EDITOR
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    MessageDisplay(Enum_ShopMessageType.Welcome);
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    MessageDisplay(Enum_ShopMessageType.Buy);
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    MessageDisplay(Enum_ShopMessageType.NoMoney);
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    MessageDisplay(Enum_ShopMessageType.InventoryFull);
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    MessageDisplay(Enum_ShopMessageType.SeeYou);
        //}
    }
#endif
}