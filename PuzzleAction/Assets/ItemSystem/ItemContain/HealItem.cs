using UnityEngine;

[CreateAssetMenu(fileName = "HealItem", menuName = "Scriptable Objects/Datas/HealItem")]
public class HealItem : Item
{
    //[SerializeField] private ItemData ItemName;
    public override void Activation(float value, ItemRecieveData data)
    {
     
        //‰ñ•œˆ—
        /* HP = Mathf.Min(HP + itemvalue, maxHP);*/
        Debug.Log($"g—p‚µ‚ÄHP‚ª{value}‰ñ•œ‚µ‚½");
    }

   
}


