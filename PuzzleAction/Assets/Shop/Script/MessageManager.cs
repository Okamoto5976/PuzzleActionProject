using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using Unity.VisualScripting;

public class MessageManager : MonoBehaviour
{
    [SerializeField] private MessageSO[] m_messages;
    [SerializeField] private TextMeshProUGUI m_speakerText;
    [SerializeField] private GameObject m_speechParent ;
    [SerializeField] private UnityEngine.UI.Button m_button;
    private int m_index = 0;
    private TextDisplay_02 m_textDisplay;

    private void Awake()
    {
        if (m_textDisplay == null)
        {
            m_textDisplay = GetComponent<TextDisplay_02>();

        }
    }

   private  void Start()
    {
        m_speechParent.SetActive(true);
        if (m_button != null)
        {
            m_button.onClick.AddListener(ShowCloseMessage);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestMessage();
        }
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
        string message = itemName + "を" + count + "個購入しました。";
        if(m_textDisplay != null)
        {
            m_textDisplay.ShowMessageGradually(message);
        }

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
        if (list.Count == 0)
        {
            Debug.Log("ランダムメッセージがありません");
            return;
        }

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
        if (m_textDisplay != null)
        {
        m_textDisplay.ShowMessageGradually(data.message);
        }

        if (m_speakerText != null)
        {
            m_speakerText.text = data.speaker;
        }
    }

    void EndMessage()
    {
        m_speechParent.SetActive(false);
        if (m_textDisplay != null)
        {
            m_textDisplay.ShowMessage("");
        }

        if (m_speakerText != null)
        {
            m_speakerText.text = "";
    }
}
    private void TestMessage()
    {
        m_speechParent.SetActive(true);

        if (m_textDisplay != null)
        {
            m_textDisplay.ShowMessageGradually("テストメッセージです");
        }

        if (m_speakerText != null)
        {
            m_speakerText.text = "NPC";
        }
    }

}