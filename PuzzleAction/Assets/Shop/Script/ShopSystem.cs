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
        for (int i = 0; i < shopButtons.Length; i ++)
        {
           // shopButtons[i].ItemId = items[i];
           // shopButtons[i].ItemName = "Item" + items[i];
           // shopButtons[i].Price = items[i] * 100;
        }
        //    int index = 0;
        //int[,] shop = new int[3, 3];
        //for (int y = 0; y < 3; y++)
        //{
        //    for (int x = 0; x < 3; x++)
        //    {
        //        shop[y, x] = items[index];
        //        index++;
        //        Debug.Log(shop[y, x]);
        //    }
        //}
    }
}

        // int[] items =
        // {
        //     1, 2, 3,
        //     4, 5, 6,
        //     7, 8, 9,
        // };
        // Shuffle(items);
        // int[,] shop = new int[3, 3];
        // int index = 0;
        // for (int y = 0; y < 3; y++)
        // {
        //     for (int x = 0; x < 3; x++)
        //     {
        //         shop[y, x] = items[index];
        //         index++;
        //         Debug.Log(shop[y, x]);
        //     }
        // }
        //}
        // void Shuffle(int[] array)
        // {
        //     for (int i = array.Length - 1; i > 0; i--)
        //     {
        //         int rand = Random.Range(0, i + 1);
        //         int temp = array[i];
        //         array[i] = array[rand];
        //         array[rand] = temp;
        //     }
        // }
        //int[,] Shop = new int[3, 3];
        //
        //int[,] shop =
        //{
        //   { 1, 2, 3 },
        //   { 4, 5, 6 },
        //   { 7, 8, 9 }
        //};
        //for (int y = 0; y < shop.GetLength(0); y++)
        //{
        //    for (int x = 0; x < shop.GetLength(1); x++)
        //    {
        //        Debug.Log(shop[y, x]);
        //
        //    }
        //}