using UnityEngine;
[CreateAssetMenu(fileName = "AttackItem", menuName = "Scriptable Objects/Datas/AttackItem")]
public class AttackItem:Item
{
    //[SerializeField] private ItemData ItemName;
    //[SerializeField] private float specialEffectDuration; //“ÁŽêŒø‰ÊŽžŠÔ
    public override void Activation(ItemRecieveData data)
    {
        //Collider
        //Pos
        //Effect

        Debug.Log($"AttackItem‚ðŽd—l‚µ‚Ü‚µ‚½");
    }
}
