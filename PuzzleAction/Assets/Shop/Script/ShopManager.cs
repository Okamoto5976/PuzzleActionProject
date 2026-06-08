using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//==============Test================
[System.Serializable]
public class TestItemData
{
    public int m_id;
    public string m_name;
    public int m_price;
    [TextArea(2, 10)]
    public string m_info;
    public Sprite m_icon;
}
//=================================

//ここはSOのデータを確認

public class ShopManager : MonoBehaviour
{
    [Header("TestItemDataManager")]
    [SerializeField] private List<TestItemData> t_itemData;


    private List<TestItemData> t_itemDataList = new();

    [SerializeField] private int m_itemNumber;

    [SerializeField] private List<Goods> m_goodsPrefab;


    [SerializeField] private TextMeshProUGUI m_moneyText;

    //仮　MoneyのDataSOを持つ
    [SerializeField] private int m_money;

    //InfoText Prefab
    [SerializeField] private InfoText m_infoTextPrefab;

    //MessageManagerを呼ぶ
    [SerializeField] private MessageManager m_messageManager;

    private DataManager m_dataManager;

    private void OnEnable()
    {
        m_messageManager.StartSequence();
        m_messageManager.ShowMessageByType(MessageType.ShopEnter);
    }

    private void OnDisable()
    {
        m_messageManager.ShowMessageByType(MessageType.ShopExit);
        gameObject.SetActive(false);
       // m_messageManager.ResetTimer();
    }

    //仮　Initializeで呼ぶ
    private void Awake()
    {
        m_dataManager = FindObjectOfType<DataManager>();
        m_infoTextPrefab.gameObject.SetActive(false);

        m_moneyText.text = "money :" + m_money.ToString();


        for(int i = 0; i < m_itemNumber; i++)
        {
            int index = UnityEngine.Random.Range(0, t_itemData.Count);

            t_itemDataList.Add(t_itemData[index]);

        }

        T_InitData();

    }

    private void T_InitData()
    {

        for(int i = 0; i < m_itemNumber;i++)
        {
            m_goodsPrefab[i].Init(t_itemDataList[i], this);

        }

    }

    //private List<ItemData> m_list = new();

    //ショップが開かれる際に呼ばれること
    //Awakeの一部をこちらに移植
    //ItemManagerの関数[ランダムにItemDataを渡す]を呼びDataを受け取る
    //Listに格納
    public void Initialize()
    {
        //for(int i = 0; i < mk_itemNumber; i++)
        //{
        //ItemData data = ItemManager.GetItem();
        //
        //m_list.Add(data);
        //
        //list[index]をm_goodPrefab[index]に
        //m_goodsPrefab[i].Init(data);
        //
        //
        //}
    }

    public bool PurchaseItem(TestItemData data)
    {
        //少ない　購入出来ない場合
        if(data.m_price > m_money)
        {
            m_messageManager.ShowMessageByType(MessageType.NoMoney);
            Debug.Log("you do not have money");
            return false;
        }
        else
        {
            Debug.Log("you purchase item");

            m_money -= data.m_price;

            m_moneyText.text = "money :" + m_money.ToString();//再び最新を表示


            //アイテム購入メッセージ表示
            m_dataManager.AddItem(data.m_id);
            m_messageManager.ShowMessageByType(MessageType.Buy);

            m_messageManager.ResetTimer();

            return true;
        }
    }

    //説明文表示
    public void OnInfoPanelFromGoods(TestItemData data)
    {
        m_infoTextPrefab.gameObject.SetActive(true);
        m_infoTextPrefab.GetItemDataInfo(data);
    }

    //説明文非表示
    public void OffInfoPanelFromGoods()
    {
        m_infoTextPrefab.Reset();
        m_infoTextPrefab.gameObject.SetActive(false);
    }
}
