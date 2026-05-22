using UnityEngine;

//public enum State
//{
//    Welcome,    //入店
//    None,       // 通常会話
//    Buy,        // 購入成功
//    NoMoney,    // お金なし
//    InventoryFull, // インベントリ満タン
//    SeeYou     //退店
//}

[CreateAssetMenu(fileName = "MessageSO", menuName = "Scriptable Objects/MessageSO")]
public class MessageSO : ScriptableObject
{
    //public State state_;        // 状態　Debug用でenumで作っているので状況に応じてstringなどに変更可能
    public string speaker;      // 話者

    [TextArea(2, 10)]
    public string message;      // 本文
}
