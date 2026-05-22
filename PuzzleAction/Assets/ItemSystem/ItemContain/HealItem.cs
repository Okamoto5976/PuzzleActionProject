
using UnityEngine;

[CreateAssetMenu(fileName = "HealItem", menuName = "Scriptable Objects/Datas/HealItem")]

public class HealItem : Item
{


    //[SerializeField] private ItemData ItemName;
    [SerializeField] private float HealingInterval;
    [SerializeField] private float HealTime;


    public override void Activation(float value, ItemRecieveData data)
    {


        if (HealTime > 0)
        {
            //ˆê’èŠÔ‰ñ•œ‚ª‚ ‚éê‡‚Ìˆ—
            while (HealTime > 0)
            {
                HealTime -= Time.deltaTime;
                //yield return new WaitForSeconds(HealingInterval);
                // HP = Mathf.Min(HP + itemvalue, maxHP);
                // return HP/;
            }
        }
        else
        {
            //’P”­‰ñ•œê‡‚Ìˆ—
            /* HP = Mathf.Min(HP + itemvalue, maxHP);*/
            //return HP;
        }          
     
        //‰ñ•œˆ—

        Debug.Log($"g—p‚µ‚ÄHP‚ª{value}‰ñ•œ‚µ‚½");
    }

   
}


