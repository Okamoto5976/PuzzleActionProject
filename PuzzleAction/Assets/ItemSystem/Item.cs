//using Unity.Multiplayer.Center.Common.Analytics;
//using System.Net.NetworkInformation;
using UnityEngine;
using static ItemManager;

public struct ItemRecieveData
{
    //public Entity entity;
    public float baseValue; //Entity—p@—áj–î‚ÌUŒ‚—Í{Entity‚ÌUŒ‚—Í
    public Vector3 pos;
    public Vector3 dir;//Œü‚«
    public Vector2 size;
}

abstract public class Item : ScriptableObject
{
    [SerializeField] private float m_value;

    [SerializeField]protected ItemData m_data;

    protected ItemPool itemPool;
    public int Id => m_data.ItemID;

    //ID@ŒöŠJ

    

    public void RecieveData(int id, ItemRecieveData data) //Entity‚ğˆø”‚É
    {

        Activation(m_value, data);
    }

    public virtual void Activation(float value, ItemRecieveData data) { }
    
 
}
