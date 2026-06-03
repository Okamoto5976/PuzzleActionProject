//using Unity.Multiplayer.Center.Common.Analytics;
//using System.Net.NetworkInformation;
using UnityEngine;

public struct ItemRecieveData
{
    //public Entity entity;
    public float baseValue; //Entity用　例）矢の攻撃力＋Entityの攻撃力
    public Vector3 pos;
    public Vector3 dir;//向き
    public Vector2 size;
}

abstract public class Item : ScriptableObject
{
    [SerializeField] private float m_value;

    [SerializeField]protected ItemData m_data;
    //private dropPool pool;
    public int Id => m_data.ItemID;
    public ItemData Data => m_data;

    //ID　公開

    public void RecieveData(int id, ItemRecieveData data) //Entityを引数に
    {

        Activation(m_value, data);
    }

    public virtual void Activation(float value, ItemRecieveData data) { }
    
 
}
/*using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private ItemPoolManager itemPool;

    public void OnDeath()
    {
        // 敵の位置にアイテムをドロップ
        itemPool.DropItem(transform.position);
        Destroy(gameObject);
    }
}
*/