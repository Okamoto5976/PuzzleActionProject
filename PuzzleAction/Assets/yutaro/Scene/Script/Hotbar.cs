using UnityEngine;

public class Hotbar : MonoBehaviour
{
    //public PlayerItem[] slots = new PlayerItem[3];
    [SerializeField] private PlayerData m_playerData;

    public PlayerItem[] slots;

    private void Awake()
    {
        //ここでサイズ決める
        slots =new PlayerItem[m_playerData.hotbarSize];
    }
    void Update()
    {
        //スロット選択
        for(int i=0;i<slots.Length;i++)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1+i))
            {
                UseItem(i);
            }
        }

    }

    void UseItem(int index)
    {
        if (index < 0 || index >= slots.Length) return;

        PlayerItem item = slots[index];
        if (item != null)
        {
            item.Use(gameObject);

            //使いきりなら消す
            slots[index] = null;
        }

    }


    public bool AddItem(PlayerItem newItem)
    {
        for(int i=0;i<slots.Length; i++)
        {
            if (slots[i]==null)
            {
                slots[i] = newItem;
                return true;//成功
            }
        }
        return false;//失敗
        Debug.Log("いっぱいで拾えない");
    }
}