using System;
using System.Collections.Generic;
using UnityEngine;

public class Rate
{
    public Data Name;
    public Data DropRate;
    public Data ItemGrad;
}

public class RandomTest : MonoBehaviour
{
    enum Rate
    {
        Name,
        DropRate,
        ItemGrad
    }
    [SerializeField]
    ItemManager PlayerItems;
    [SerializeField]
    ItemManager ItemRate;
    //public List<Rate> Rates = new();
    // public List<Data> DropList = new();

    float[,] RarityInfo;
    string[,] itemInfo;
    string[] raw;
    string[] column;
    int rollNum = 0;
    int lotteryType = 0;
    public Data data;
    private void Start()
    {
        //raw;
        column = raw[0].Split(',');
        RarityInfo = new float[column.Length, raw.Length];
        for (int i = 0; i < raw.Length; i++)
        {
            column = raw[i].Split(',');
            for (int j = 0; j < column.Length; j++)
                RarityInfo[j, i] = float.Parse(column[j]);
        }
        //raw
        column = raw[0].Split(',');
        for (int i = 0; i < raw.Length; i++)
        {
            column = raw[i].Split(',');
            for (int j = 0; j < column.Length; j++)
                itemInfo[j, i] = column[j];
        }
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < rollNum; i++)
            {
                GetDropItem();
            }

            return;
        }
    }

    void GetDropItem()
    {
        //レア度の抽選
        int itemRarity = ChooseRarity() + 3;
        //レア度に応じたアイテムの抽選
        int itemId = ChooseItem(itemRarity);
        Debug.Log(itemInfo[(int)Rate.Name, itemId]);
    }
    int ChooseRarity()
    {
        float total = 0;
        for (int i = 0; i < RarityInfo.GetLength(1); i++)
            total += RarityInfo[i, lotteryType];


        float randomPoint = UnityEngine.Random.value * total;
        for (int i = 0; i < itemInfo.GetLength(0); i++)
        {
            if (randomPoint < RarityInfo[i, lotteryType])
            {
                return i;
            }
            else
            {
                randomPoint -= RarityInfo[i, lotteryType];
            }
        }
        return 0;
    }
    int ChooseItem(int rarity)
    {
        float total = 0;
        for (int i = 0; i < itemInfo.GetLength(1); i++)
            if (int.Parse(itemInfo[(int)Rate.ItemGrad, i]).Equals(rarity))
                total += float.Parse(itemInfo[(int)Rate.DropRate, i]);
        float randomPoint = UnityEngine.Random.value * total;
        for (int i = 0; i < itemInfo.GetLength(1); i++)
        {
            if (int.Parse(itemInfo[(int)Rate.ItemGrad, i]).Equals(rarity))
            {
                if (randomPoint < float.Parse(itemInfo[(int)Rate.DropRate, i]))
                {
                    return i;
                }
                else
                {
                    randomPoint -= float.Parse(itemInfo[(int)Rate.DropRate, i]);
                }
            }
        }
        return 0;
    }




}
