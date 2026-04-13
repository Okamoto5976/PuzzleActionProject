using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    private void Start()
    {
        int[,] Shop = new int[3, 3];
        //Shop[0,0] = 1;      //ç∂è„
        //Shop[1,2] = 5;      //ê^ÇÒíÜâE

        int[,] shop =
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 9 }
        };
        for (int y = 0; y < shop.GetLength(0); y++)
        {
            for (int x = 0; x < shop.GetLength(1); x++)
            {
                Debug.Log(shop[y,x]);

            }
        }
    }
}
