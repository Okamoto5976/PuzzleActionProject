using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] public ItemSlot[,] slots = new ItemSlot[3, 3]; //マス生成
    private void Awake()
    {
        //全マス初期化
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                slots[y, x] = new ItemSlot();
            }
        }
    }
    
    //アイテムを追加する関数
    public bool AddItem(int itemId)
    {
        //同じアイテムがあれば重ねる
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if (slots[y, x].m_itemId == itemId)
                {
                    slots[y, x].m_count++;
                    Debug.Log($"アイテム{itemId}を重ねた({slots[y, x].m_count}個)");
                    return true;
                }
            }
        }
        //空いてるマスを探す
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if (slots[y, x].m_itemId == 0)
                {
                    slots[y, x].m_itemId = itemId;
                    slots[y, x].m_count = 1;
                    Debug.Log($"新しいアイテム{itemId}を追加");
                    return true;
                }
            }
        }
        //どこにも入らない
        Debug.Log("インベントリがいっぱい");
        return false;
    }
}
