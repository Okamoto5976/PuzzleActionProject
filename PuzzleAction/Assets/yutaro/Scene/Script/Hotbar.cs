using UnityEngine;

public class Hotbar : MonoBehaviour
{
    public PlayerItem[] slots = new PlayerItem[3];


    void Update()
    {
        //スロット選択
        if (Input.GetKeyDown(KeyCode.Alpha1)) UseItem (0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) UseItem (1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) UseItem (2);

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