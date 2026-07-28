using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MessageSO_Shop", menuName = "Scriptable Objects/MessageSO_Shop")]
public class MessageSO_Shop : ScriptableObject
{
    [SerializeField] public List<MessageList> m_messageList;
}

[System.Serializable]
public class MessageList
{
    public string m_messages;
    public Enum_ShopMessageType m_messageType;
}

