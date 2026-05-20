using UnityEngine;
[CreateAssetMenu(fileName = "SpecialItem", menuName = "Scriptable Objects/Datas/SpecialItem")]
public class OthorItem:Item
{
    [SerializeField] private ItemData ItemName;
    [SerializeField] private float specialEffectDuration; //“ÁêŒø‰ÊŠÔ
    public override void Activation(float value, ItemRecieveData data)
    {

        Debug.Log($"{ItemName}‚ªg‘ã‚í‚è‚É‚È‚è‚Ü‚µ‚½I");
    }
}
