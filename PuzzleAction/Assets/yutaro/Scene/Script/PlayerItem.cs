using UnityEngine;

public abstract class PlayerItem : ScriptableObject
{
    //アイテムの名前
    public string name = "New Item";

    //アイテムのアイコン
    public Sprite icon = null;

    //アイテムの使用
    public abstract void Use(GameObject user);
  
    
}
