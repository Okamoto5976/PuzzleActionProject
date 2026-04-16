using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class InventorySystem : MonoBehaviour
{
    [Header("UI設定")]
    public TextMeshProUGUI m_descriptionText;//説明文を表示するテキストUI
    public GameObject m_infoPanel;

    [Header("Inventory設定")]
    public Vector2 m_inventoryOrigin;
    public int m_slotSize = 64;

    [Header("Inventory UI")]
    public GameObject m_inventoryPanel;
    public GameObject m_addButton;

    [Header("Slot Images")]
    public Image[] m_slotImages;

    [Header("Item Images")]
    public Sprite m_potionSprite;

    


    //private Camera _mainCamera;

    private Item[,] m_inventory;
    void Start()
    {
        // インベントリ初期化(3行×10列)
        m_inventory = new Item[3, 10];//inspectorでいじれるように

        // テスト用アイテムを入れる
        //m_inventory[0, 0] = new Item("Potion", "HPを50回復する");
        //m_inventory[1, 2] = new Item("剣", "攻撃力が上がる");

        m_infoPanel.SetActive(false);//最初は非表示
       
    }

    
    private void Update()
    {

        if(!m_isInventoryOpen)
        {
            HideDescription();
            return;
        }

        Vector2 mousePos = Input.mousePosition;
        
        // スクリーン座標→UI基準Y座標へ変換
        float uiMouseY = Screen.height - mousePos.y;

        //Debug.Log($"mousePos : {mousePos}");

        Debug.LogWarning($"uiMouseY : {uiMouseY}");

        int x = (int)((mousePos.x - m_inventoryOrigin.x) / m_slotSize);
        int y = (int)((uiMouseY - m_inventoryOrigin.y) / m_slotSize);

        // 配列範囲内チェック
        if (x >= 0 && x < m_inventory.GetLength(1) &&
            y >= 0 && y < m_inventory.GetLength(0))
        {
            Item item = m_inventory[y, x];

            if (item != null)
            {
                ShowDescription(item);
                return;
            }
        }

        // どの条件にも当てはまらなかったら非表示
        HideDescription();

        //transform.position = worldpos;

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        Debug.Log(hit.point);
        //        Debug.Log(hit.collider.name);
        //    }
        //}
    }

    private bool m_isInventoryOpen = false;

    public void ToggleInventory()
    {
        Debug.Log("Inventoryボタンが押された");
        m_isInventoryOpen = !m_isInventoryOpen;

        m_inventoryPanel.SetActive(m_isInventoryOpen);
        m_addButton.SetActive(m_isInventoryOpen);

        // 閉じたら説明も消す
        if (!m_isInventoryOpen)
        {
            HideDescription();
        }
    }
    public void AddItemButton()
    {
        Item newItem = new Item("Potion", "HPを50回復する");

        int columns = m_inventory.GetLength(1);

        for (int y = 0; y < m_inventory.GetLength(0); y++)
        {
            for (int x = 0; x < m_inventory.GetLength(1); x++)
            {
                if (m_inventory[y, x] == null)
                {
                    m_inventory[y, x] = newItem;

                    int slotIndex = y * columns + x;

                    if (slotIndex < m_slotImages.Length)
                    {
                        m_slotImages[slotIndex].sprite = m_potionSprite;
                        m_slotImages[slotIndex].color = Color.white;
                    }

                    Debug.Log($"Item追加({y}, {x}) / Slot {slotIndex}");
                    return;
                }
            }
        }

        Debug.Log("インベントリが満杯です");

    }

    void ShowDescription(Item item)
    {
        m_infoPanel.SetActive(true);
        m_descriptionText.text = item.description;
    }

    void HideDescription()
    {
        m_infoPanel.SetActive(false);
    }
   
}
