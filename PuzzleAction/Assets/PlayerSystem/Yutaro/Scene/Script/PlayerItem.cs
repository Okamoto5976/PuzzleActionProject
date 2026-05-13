using UnityEngine;

public abstract class PlayerItem : ScriptableObject
{

    [SerializeField] private string m_itemname;
    [SerializeField] private Sprite m_icon;
    //アイテムの名前
    public string Itemname => m_itemname;

    //アイテムのアイコン
    public Sprite icon => m_icon;

    //アイテムの使用
    public abstract void Use(GameObject user);
  
    
}
