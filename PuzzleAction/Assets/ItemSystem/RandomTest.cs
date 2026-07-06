using System.Collections.Generic;
using UnityEngine;
using System;
public enum RarityRate
{
    Nomal,
    Rara,
    SparRara
}
[System.Serializable]
public class Rate
{
    public Item Name;
    public Item DropRate;
    public RarityRate rarity;
    public int weight = 1;
}


public class RandomTest:MonoBehaviour
{

    public List<Rate> Items = new();
     //public InfoText Rate;

    [Header("")]
    [Range(0, 100)] public float Nomal = 60f;
    [Range(0, 100)] public float Rara = 30f;
    [Range(0, 100)] public float SparRara = 10f;

    public Rate DrawItem()
    {

        RarityRate rarity = GetRandomRarity(Items);
        List<Rate> candidates = Items.FindAll(Rate => Rate.rarity == rarity);

        if (candidates.Count == 0)
        {
            Debug.Log($"{rarity}のアイテムがありません。再抽選します。");
            return DrawItem();
        }

        int index = UnityEngine.Random.Range(0, candidates.Count);
        return candidates[index];

    }

    private RarityRate GetRandomRarity(List<Rate> candidates)
    {
        float rand =UnityEngine. Random.Range(0f, 100f);

        if (rand < Nomal)
            return RarityRate.Nomal;

        rand -= Nomal;

        if (rand < Rara)
            return RarityRate.Rara;

        rand -= Rara;

        if (rand < SparRara)
            return RarityRate.SparRara;

        return RarityRate.SparRara;

        

    }



    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rate rate= DrawItem();
            Debug.Log($"獲得アイテム：{rate.Name} ({rate.rarity})");
        }

    }

}
 
