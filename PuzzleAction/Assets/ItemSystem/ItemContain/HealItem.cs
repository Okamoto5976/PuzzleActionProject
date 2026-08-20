using UnityEngine;

[CreateAssetMenu(fileName = "HealItem", menuName = "Scriptable Objects/Datas/HealItem")]

public class HealItem : Item
{
    [SerializeField] private float HealingInterval;
    //[SerializeField] private float HealTime;


    public override void Activation(float value, ItemRecieveData data)
    {

        data.entity.HealHP(value);


        //if (HealTime > 0)
        //{
        //    //ˆê’èŠÔ‰ñ•œ‚ª‚ ‚éê‡‚Ìˆ—
        //    while (HealTime > 0)
        //    {
        //        HealTime -= Time.deltaTime;
        //        //yield return new WaitForSeconds(HealingInterval);
        //        //entity = Mathf.Min(entity + itemvalue, maxHP);
        //        //return entity;
        //    }
        //}
        //else
        //{
        //    //’P”­‰ñ•œê‡‚Ìˆ—
        //    /* entity = Mathf.Min(entity + itemvalue, maxHP);*/
        //    //return entity;
        //}          
     
        //‰ñ•œˆ—

        Debug.Log($"g—p‚µ‚ÄHP‚ª{value}‰ñ•œ‚µ‚½");
    }

   
}


