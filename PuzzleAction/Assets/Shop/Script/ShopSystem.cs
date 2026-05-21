using UnityEngine;
using System.Collections.Generic;

public class ShopSystem : MonoBehaviour
{
    [SerializeField] private Button[] shopButtons;

    private void Start()
    {
        List<int> items = new List<int>
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9,
        };
        //ƒVƒƒƒbƒtƒ‹
        for (int i = 0; i < items.Count; i++)
        {
            int rand = Random.Range(i, items.Count);

            int temp = items[i];
            items[i] = items[rand];
            items[rand] = temp;
        }
        for (int i = 0; i < shopButtons.Length; i++)
        {
            // shopButtons[i].ItemId = items[i];
            // shopButtons[i].ItemName = "Item" + items[i];
            // shopButtons[i].Price = items[i] * 100;
        }
    }
}