using UnityEngine;

public struct ItemRecieveData
{
    //public Entity entity;
    public float baseValue; //Entity�p�@��j��̍U���́{Entity�̍U����
    public Vector3 pos;
    public Vector3 dir;//����
    public Vector2 size;
}

abstract public class Item : ScriptableObject
{
    [SerializeField] private float m_value;

    [SerializeField]protected ItemData m_data;
    //private dropPool pool;
    public int Id => m_data.ItemID;
    public ItemData Data => m_data;

    //ID�@���J

    public void RecieveData(int id, ItemRecieveData data) //Entity��������
    {
        
        Activation(m_value, data);
    }

    public virtual void Activation(float value, ItemRecieveData data) { }
    
 
}
