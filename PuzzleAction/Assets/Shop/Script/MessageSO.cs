using UnityEngine;

public enum MessageType
{
    ShopEnter,    //入店
    Normal,       // 通常会話
    Buy,        // 購入成功
    NoMoney,    // お金なし
    InventoryFull, // インベントリ満タン
    ShopExit     //退店
}

[CreateAssetMenu(fileName = "MessageSO", menuName = "Scriptable Objects/MessageSO")]
public class MessageSO : ScriptableObject
{
    public MessageType m_messageType;  
    public string speaker;      // 話者

    [TextArea(2, 10)]
    public string message;      // 本文
}
