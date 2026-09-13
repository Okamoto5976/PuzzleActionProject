using UnityEngine;
[CreateAssetMenu(fileName = "OtherItem", menuName = "Scriptable Objects/Datas/OtherItem")]
public class OthorItem:Item
{
    //[SerializeField] private ItemData ItemName;
    //[SerializeField] private float specialEffectDuration; //“ÁŽêŒø‰ÊŽžŠÔ

    [SerializeField] private Passive m_passiveType;

    public override void Activation(ItemRecieveData data)
    {
        //Passive
        Debug.Log($"Passive ”­“®");
    }
}
