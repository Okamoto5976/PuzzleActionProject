using UnityEngine;
[CreateAssetMenu(fileName = "DeBuffItem", menuName = "Scriptable Objects/Datas/DeBuffItem")]
public class DeBuff : Item
{
    public enum DeBuffType
    {
        AttackDown,
        DefenseDown,
        SpeedDown,
        // 他のデバフの種類を追加
    }
   
    [SerializeField] private float deBuffDuration; //デバフ効果時間
    [SerializeField] private DeBuffType deBuffType; //デバフの種類
    //[SerializeField] public ItemData ItemName;

    public override void Activation(float value, ItemRecieveData data)
     {
        //デバフ処理
        switch (deBuffType)
        {
            case DeBuffType.AttackDown:
                //Power =(Power-value,DefaultPower);
                break;
            case DeBuffType.DefenseDown:
                //Defense= (Defense-value, DefaultDefense);
                break;
            case DeBuffType.SpeedDown:
                //speed = (Speed-value, DefaultSpeed);
                break;
            default:
                break;
        }
        Debug.Log($"使用して{deBuffDuration}秒間、{deBuffType}が{value}減少した");

    }

}
