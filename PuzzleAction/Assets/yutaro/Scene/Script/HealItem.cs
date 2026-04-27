using UnityEngine;

[CreateAssetMenu(menuName ="Items/HealItem")]
public class HealItem:PlayerItem
{
    public int healAmount = 20;
    
    public override void Use(GameObject user)
    {
        var hp =user.GetComponent<EntityHP>();

        if(hp!=null)
        {
            hp.Heal(healAmount);
        }

        Debug.Log(name + "‚Å‰ñ•œ‚µ‚½");
    }
}
