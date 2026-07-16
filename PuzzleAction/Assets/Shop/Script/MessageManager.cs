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

    /// <summary>
    /// Enumからメッセージ取得
    /// </summary>
    private MessageList GetMessage(Enum_ShopMessageType type)
    {
        if (m_messages == null)
        {
            Debug.LogError("MessageSO_Shopが設定されていません");
            return null;
        }

        foreach (MessageList message in m_messages.m_messageList)
        {
            if (message.m_messageType == type)
            {
                return message;
            }
        }

        return null;
    }

    /// <summary>
    /// メッセージ表示
    /// </summary>
    public void MessageDisplay(Enum_ShopMessageType type)
    {
        MessageList message = GetMessage(type);

        if (message == null)
        {
            Debug.LogWarning($"{type} のメッセージが見つかりません");
            return;
        }

        switch (type)
        {
            case Enum_ShopMessageType.Welcome:
                Debug.Log("入店");
                break;

            case Enum_ShopMessageType.None:
                Debug.Log("通常会話");
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
                Debug.LogWarning("未対応のMessageType");
                break;
        }

        ShowMessage(message.m_messages);
    }

    /// <summary>
    /// 同じTypeの中からランダム表示
    /// </summary>
    public void MessageDisplayRandom(Enum_ShopMessageType type)
    {
        List<MessageList> list = new();

        foreach (MessageList message in m_messages.m_messageList)
        {
            if (message.m_messageType == type)
            {
                list.Add(message);
            }
        }

        if (list.Count == 0)
        {
            Debug.LogWarning($"{type} のメッセージがありません");
            return;
        }

        MessageList randomMessage =
            list[Random.Range(0, list.Count)];

        ShowMessage(randomMessage.m_messages);
    }

    /// <summary>
    /// 購入専用
    /// </summary>
    public void ShowBuyMessage(string itemName, int count)
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
            string message =
                $"{itemName}を{count}個購入しました。";

            m_textDisplay.ShowMessageGradually(message);
        }
    }

    /// <summary>
    /// メッセージ表示共通処理
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
    /// メッセージ閉じる
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

    // テスト
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MessageDisplay(Enum_ShopMessageType.Welcome);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MessageDisplay(Enum_ShopMessageType.Buy);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MessageDisplay(Enum_ShopMessageType.NoMoney);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            MessageDisplay(Enum_ShopMessageType.SeeYou);
        }
    }
}