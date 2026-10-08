using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    //Playerが生成されてフラグ　Playerの操作方法　＋　回避

    //ShopAreaに入ると　Shopに行くよう　促す　（見えない壁を生成）当たるとフラグ　Shopに入りましょう
    //Shopを開いた際にフラグ
    //ここではアイテムをお金で買うことができます
    //退店ボタンで　Tabを押せるようにする（押すまで操作不能）
    //インベントリを開くと　説明
    //アイテムをホットバーに入れることができます
    //まずアイテムをクリックしてください　（このチュートリアル終わるまで　CloseやTab、Playerの動き＋Timeを0に
    //クリックを検知で　次に　説明と　アイテムの使用法が書かれています
    //次に　上のボタンで　選択したアイテムの廃棄　左上のホットバーにいれることができます
    //1,2,3で　ホットバーを切り替え　アイテムを使用できます
    //
    //下でPlayerのステータスを確認できます
    //？を押すと　詳細を出せます
    //これで　インベントリのチュートリアルを終わる
    //
    //TrapAreaに近づいたときフラグ　
    //Trapは　基本使用者には　ききません（爆弾は　例外です）
    //このTrapAreaには　Player　敵　関係なくダメージが発生します
    //このAreaを使用し　敵を倒しましょう

    //EnemyAreaに近づいたとき　フラグ
    //このEnemyAreaでは　Playerを襲う　モンスターが現れます
    //倒すことで　アイテムや　お金を入手できます
    //ゴールまで行くと　このステージをクリアできます　敵を倒したりする必要はありません
    //
    //ゴールして　チュートリアルのフラグを解除していく
    //次のマップ選択で　妖精の泉と　Ｂｏｓｓは倒さないと　ゴールできないことを説明

    //塔をどれだけ上れるか　挑戦しましょう

    //component
    [SerializeField] private PlayerController m_playerController;
    [SerializeField] private EntityHP m_playerHP;

    [SerializeField] private GameObject m_backPanel;

    [SerializeField] private AudioData m_clickSE;

    private void Start()
    {
        if (!GameManager.Instance.IsTutorial) return;

        Debug.LogWarning("Tutorial Start");
        m_canTab = false;
        m_canClickItem = false;
        m_playerHP.m_isTutorial = true;
    }

    private void Update()
    {
        if(!GameManager.Instance.IsTutorial) return;

        var pos = m_playerController.gameObject.transform.position;


        if (!m_isShopEvent)
        {
            if (pos.z > m_shopEventZPos)
            {
                ShopAreaTutorial();
            }
        }

        if(!m_isAttentionEvent)
        {
            if(pos.z > m_attentionZPos)
            {
                m_playerController.gameObject.transform.position = new Vector3(pos.x, pos.y, m_attentionZPos);
            }

            if (pos.z <= m_attentionZPos -1 && pos.z > m_attentionZPos - 5f)
            {
                AttentionShopMessage(true);
            }
            else
            {
                AttentionShopMessage(false);

            }
        }

        if(!m_isTrapEvent)
        {
            if (pos.z > m_trapEventZPos)
            {
                TrapAreaTutorial();
            }
        }

        if(!m_isEnemyEvent)
        {
            if(pos.z > m_enemyEventZPos)
            {
                EnemyAreaTutorial();
            }
        }
    }

    private float m_shopEventZPos;
    private float m_attentionZPos;
    private float m_trapEventZPos;
    private float m_enemyEventZPos;

    private bool m_isShopEvent;
    private bool m_isAttentionEvent;
    private bool m_isTrapEvent;
    private bool m_isEnemyEvent;

    public void SetShopEventPos(float zPos) => m_shopEventZPos = zPos;
    public void SetAttentionPos(float zPos) => m_attentionZPos = zPos;
    public void SetTrapEventPos(float zPos) => m_trapEventZPos = zPos;
    public void SetEnemyEventPos(float zPos) => m_enemyEventZPos = zPos;

    [Header("PlayerControllerTutorial")]
    [SerializeField] private GameObject m_playerControllerTutorialPanel;
    private bool m_PCT = false;

    public void PlayerControllerTutorial()
    {
        //true
        m_PCT = !m_PCT;
        //画面表示、PlayerをCanMove = false


        //false
        //戻す

        if(m_PCT)
        {
            m_playerController.SetCanMove(false);

            m_playerControllerTutorialPanel.SetActive(true);

        }
        else
        {
            m_playerController.SetCanMove(true);

            m_playerControllerTutorialPanel.SetActive(false);
        }

    }

    [Header("ShopAreaTutorial")]
    [SerializeField] private GameObject m_shopAreaTutorialPanel;
    private bool m_SAT = false;
    private bool m_SATActive = true;

    public void ShopAreaTutorial()
    {
        if (!m_SATActive) return;
        //true
        //画面表示、PlayerをCanMove = false
        //false
        //戻す
        m_SAT = !m_SAT;

        if(m_SAT)
        {
            m_isShopEvent = true;

            m_playerController.SetCanMove(false);

            m_shopAreaTutorialPanel.SetActive(true);
        }
        else
        {
            m_playerController.SetCanMove(true);

            m_shopAreaTutorialPanel.SetActive(false);
            m_SATActive = false;
        }
    }

    [Header("AttentionShopMessage")]
    [SerializeField] private GameObject m_attentionShopMessagePanel;
    private bool m_ASM = false;
    public void AttentionShopMessage(bool value)
    {
        if(m_ASM) return;

        //見えない壁にぶつかるとフラグ
        //Enter Exitで切り替え
        m_attentionShopMessagePanel.SetActive(value);
    }

    [Header("ShopEnter")]
    [SerializeField] private GameObject m_shopEnterTutorialPanel;
    private bool m_SET = false;
    private bool m_SETActive = true;

    //[HideInInspector] public bool m_isEnterShop = false;


    public void ShopEnterTutorial()
    {
        if (!m_SETActive) return;

        //true
        //画面表示

        //false
        //戻す
         m_SET = !m_SET;

        if(m_SET)
        {
            m_isAttentionEvent = true;
            m_ASM = true;
            m_attentionShopMessagePanel.SetActive(false);


            m_shopEnterTutorialPanel.SetActive(true);
            m_backPanel.SetActive(true);
            //m_isEnterShop = true;

        }
        else
        {
            m_shopEnterTutorialPanel.SetActive(false);
            m_SETActive = false;
            m_backPanel.SetActive(false);

        }
    }

    //購入しないと　退店できない
    [HideInInspector] public bool m_isPurchase = false;

    [Header("ShopLeave")]
    [SerializeField] private GameObject m_shopLeaveTutorialPanel;
    private bool m_SLT = false;
    private bool m_SLTActive = true;
    public void ShopLeaveTutorial()
    {
        if (!m_SLTActive) return;
        //画面表示,PlayerをCanMove = false
        //Tabを押せるようにする
        //次のメソッドから非表示する
        m_SLT = !m_SLT;

        if(m_SLT)
        {
            m_playerController.SetCanMove(false);
            m_playerController.SetCanInteract(false);

            m_canTab = true;

            m_shopLeaveTutorialPanel.SetActive(true);
        }
        else
        {
            m_shopLeaveTutorialPanel.SetActive(false);
            m_SLTActive = false;
        }
    }

    [Header("InventoryTutorial")]
    [SerializeField] private RectTransform m_inventoryTutorialPanel;
    [SerializeField] private GameObject m_button;

    [SerializeField] private TextMeshProUGUI m_text;
    [SerializeField] private List<string> m_textList = new();
    [SerializeField] private List<Vector2> m_panelPos = new();

    //passiveはこれがfalseだとはじく
    //activeでtrueに　＋　次のフラグを
    [HideInInspector] public bool m_canClickItem = true;

    int num = 0;

    private bool m_ITActive = true;
    public void InventoryTutorial()
    {
        if (!m_ITActive) return;

        m_ITActive = false;
        m_isInventoryEvent = true;
        //画面を非表示
        ShopLeaveTutorial();
        //Time = 0f
        GameManager.Instance.OnSetStop(true);
        //Inventory Close, Tabを押せないように
        m_canTab = false;
        //まずはアイテムを押しましょう表示
        m_inventoryTutorialPanel.gameObject.SetActive(true);
        m_button.SetActive(false);
        m_backPanel.SetActive(false);
        m_inventoryTutorialPanel.anchoredPosition = m_panelPos[num];
        m_text.text = m_textList[num];
        //List 0
    }
    //Tabを押せるか
    [HideInInspector] public bool m_canTab = true;
    //今インベントリのイベントで　Closeが押せないよ
    [HideInInspector] public bool m_isInventoryEvent = false;

    //メッセージをListに格納してるので　そこからメッセージを拾い　PanelのRectも操作
    //メッセージを進める手段は決める

    //ボタンでNext

    //まずはアイテムを押しましょうフラグ

    private void SetInventoryMessage(string text, Vector2 pos)
    {
        m_inventoryTutorialPanel.anchoredPosition = pos;
        m_text.text = text;

    }

  

    //説明と使用法
    //Dropとホットバー
    //ホットバー切り替え
    //ステータスの見方
    //解除
    public void InventoryTutorialNextButton()
    {
        num++;

        //もしListがないなら
        //元に戻す
        if (num >= m_textList.Count)
        {
            GameManager.Instance.OnSetStop(false);
            m_canTab = true;
            m_canClickItem = true;
            m_isInventoryEvent = false;
            m_playerController.SetCanMove(true);
            m_playerController.SetCanInteract(true);

            m_backPanel.SetActive(false);

            m_inventoryTutorialPanel.gameObject.SetActive(false);
            m_button.SetActive(false);
            m_backPanel.SetActive(false);
            return;
        }

        m_backPanel.SetActive(true);

        m_button.SetActive(true);
        m_backPanel.SetActive(true);
        //num++;
        //List num
        SetInventoryMessage(m_textList[num], m_panelPos[num]);

      
    }

    [Header("TrapAreaPanel")]
    [SerializeField] private GameObject m_trapAreaPanel;
    private bool m_TAP = false;
    public void TrapAreaTutorial()
    {
        m_TAP = !m_TAP;

        if(m_TAP)
        {
            m_isTrapEvent = true;
            m_trapAreaPanel.SetActive(true);

            m_playerController.SetCanMove(false);


        }
        else
        {
            m_trapAreaPanel.SetActive(false);

            m_playerController.SetCanMove(true);

        }

        //true
        //画面表示、PlayerをCanMove = false

        //false
        //戻す
    }

    [Header("EnemyAreaPanel")]
    [SerializeField] private GameObject m_enemyAreaPanel;
    private bool m_EAP = false;
    public void EnemyAreaTutorial()
    {
        m_EAP = !m_EAP;

        if(m_EAP)
        {
            m_isEnemyEvent = true;
            GameManager.Instance.OnSetStop(true);

            m_enemyAreaPanel.SetActive(true);
            m_playerController.SetCanMove(false);

        }
        else
        {
            GameManager.Instance.OnSetStop(false);

            m_enemyAreaPanel.SetActive(false);
            m_playerController.SetCanMove(true);

        }
        //true
        //画面表示、PlayerをCanMove = false

        //false
        //戻す
    }

    public void OnSE()
    {
        AudioManager.Instance.PlayAudio(m_clickSE);
    }


}
