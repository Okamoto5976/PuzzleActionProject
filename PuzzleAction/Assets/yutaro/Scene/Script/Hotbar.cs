using UnityEngine;

public class Hotbar : MonoBehaviour
{
    public PlayerItem[] slots = new PlayerItem[3];

    private int currentIndex=0;

    void Update()
    {
        //スロット選択
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) currentIndex = 2;

        //使用キー
        if(Input.GetKeyDown(KeyCode.E))
        {
            UseCurrentItem();
        }
    }

    public void UseCurrentItem()
    {
        PlayerItem item =slots[currentIndex];

        if (item != null)
        {
            item.Use(gameObject);
        }
    }
}