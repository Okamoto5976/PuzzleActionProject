using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class MessageManager : MonoBehaviour
{
    [SerializeField] private MessageSO[] m_messages;
    [SerializeField] private TextMeshProUGUI m_messageText;
    [SerializeField] private TextMeshProUGUI m_speakerText;
    [SerializeField] private GameObject m_speechParent ;
    [SerializeField] private UnityEngine.UI.Button m_button;
    private int m_index = 0;

    void Start()
    {
        m_speechParent.SetActive(true);
        m_button.onClick.AddListener(ShowCloseMessage);
    }

    //通常会話再生（Noneのみ）
    public void StartSequence()
    {
        m_index = 0;
        m_speechParent.SetActive(true);
        ShowNextNoneMessage();
    }
    public void ShowWelcome()
    {
        //foreach (var data in m_messages)
        //{
        //    if (data.state_ == State.None)
        //    {
        //        m_speechParent.SetActive(true);
        //        Show(data);
        //        return;
        //    }
        //}
    }
    //Noneだけ順番表示
    public void ShowNextNoneMessage()
    {
        //while (m_index < m_messages.Length)
        //{
        //    MessageSO data = m_messages[m_index];
        //    m_index++;

        //    if (data.state_ == State.None)
        //    {
        //        Show(data);
        //        return;
        //    }
        //}

        EndMessage();
    }

    //条件メッセージ表示
    public void ShowByState(State state)
    {
        //foreach (var data in m_messages)
        //{
        //    if (data.state_ == state)
        //    {
        //        m_speechParent.SetActive(true);
        //        Show(data);
        //        return;
        //    }
        //}

        Debug.Log("該当するメッセージなし: " + state);
    }
    public void ShowBuyMessage(string itemName, int count)
    {
        //吹き出しを表示
        m_speechParent.SetActive(true);
        //メッセージ作成
        m_messageText.text = itemName + "wo" + count + "Buy";
        if (m_speakerText != null)
        {
            m_speakerText.text = "Shop";
        }
    }
    public void ShowRandomNoneMessage()
    {
        var list = new
        System.Collections.Generic.List<MessageSO>();
        //foreach (var data in m_messages)
        //{
        //    if (data.state_ == State.None)
        //    {
        //        list.Add(data);
        //    }
        //}
        if (list.Count == 0) return;

        int rand = Random.Range(0, list.Count);
        m_speechParent.SetActive(true);
        Show(list[rand]);
    }
    public void ShowCloseMessage()
     {

        m_speechParent.SetActive(true);
        while (m_index < m_messages.Length)
        {
            MessageSO data = m_messages[m_index];
            m_index++;

            //if (data.state_ == State.SeeYou)
            //{
            //    Show(data);
            //    return;
            //}
        }
    }


    void Show(MessageSO data)
    {
        m_messageText.text = data.message;

        if (m_speakerText != null)
            m_speakerText.text = data.speaker;
    }

    void EndMessage()
    {
        m_speechParent.SetActive(false);
        m_messageText.text = "";
        if (m_speakerText != null) m_speakerText.text = "";
    }
}