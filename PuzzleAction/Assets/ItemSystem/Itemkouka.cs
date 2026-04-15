using JetBrains.Annotations;
using System;
using UnityEngine;


abstract class Item : MonoBehaviour
{

    [SerializeField] public ItemData ItemName;      //アイテム名
    [SerializeField] public ItemData ItemValue;            //攻撃力、回復力、防御力など
    public ItemData name;
    public ItemData value;
    protected Item(ItemData ItemName ,ItemData ItemValue)
    {
        name = ItemName;
        value = ItemValue ;

    }
    public abstract void Use();
}

/*class HealItem : Item
{
    public HealItem(ItemData ItemName, ItemData ItemValue) : base(ItemName, ItemValue)
    {
        name = ItemName;
        value = ItemValue;
    }
    public override void Use()
    {
        Console.WriteLine($"{name}{value}");
    }
}
class PowerItem : Item
{
    public PowerItem(ItemData ItemName, ItemData ItemValue):base(ItemName,ItemValue)
    {
        name = ItemName;
        value = ItemValue;
    }
    public override void Use()
    {
        Console.WriteLine($"{name}{value}");
    }
}

class BildeItem : Item
{
    public BildeItem(ItemData ItemName, ItemData ItemValue):base(ItemName,ItemValue)
    {
        name = ItemName;
        value = ItemValue;
    }
    public override void Use()
    {
        Console.WriteLine($"{name}{value}");
    }
}*/