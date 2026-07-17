using UnityEngine;

public struct ItemRecieveData
{
    public Entity entity;
    public float baseValue; //For example, arrowAttack + baseValue(EntityAttack)
    public Vector3 pos;
    public Vector3 dir;
    public Vector2 size;
}

public enum ItemType
{
    Active,
    Passive
}

abstract public class Item : ScriptableObject
{
    public enum ItemEffectType
    {
        Heal,
        Trap,
        Buff,
        Other
    }

    [SerializeField] private float m_value;
    [SerializeField] private ItemEffectType m_type;
    [SerializeField]protected ItemData m_data;
    //private dropPool pool;
    //public int Id => m_data.ItemID;
    public ItemData Data => m_data;
    public int ID => m_data.ItemID;

    public string ItemName => m_data.ItemName;

    public string info => m_data.Description;
    public Sprite icon => m_data.ItemIcon;
    public bool stackable => m_data.MaxStack > 1;

    public ItemType ItemType => m_data.ItemType;

    public ItemEffectType Type => m_type;

    public Grade grade => m_data.ItemGrade;
    //ID public

    public void RecieveData(ItemRecieveData data)
    {
        Activation(m_value, data);
    }

    public virtual void Press(ItemRecieveData data) { }

    public virtual void Hold(ItemRecieveData data) { }

    public virtual void Release(ItemRecieveData data) { }


    public virtual void Activation(float value, ItemRecieveData data) { }
    
 
}
