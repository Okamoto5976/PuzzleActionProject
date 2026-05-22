using UnityEngine;
[CreateAssetMenu(fileName = "AttackItem", menuName = "Scriptable Objects/Datas/AttackItem")]
public class AttackItem : Item
{
    private enum AttackPrefab
    {
        Effect,
        Attack
    }
    [SerializeField] public GameObject ItemPrefab;
    [SerializeField] private float WeponDameg;
    [SerializeField] private float WeponRange;
    [SerializeField] private AttackPrefab PrefabType;
    [SerializeField] private GameObject AttackObject;
    [SerializeField] private float Durability; //‘Ï‹v’l
    [SerializeField] private float attackRange; //UŒ‚”ÍˆÍ
    [SerializeField] private float Damage; //ƒ_ƒ[ƒW
    
     
    void Durabity(float durability)
    {
        
        //‘Ï‹v’l‚Ìˆ—
        Durability -= 1; //UŒ‚‚·‚é‚½‚Ñ‚É‘Ï‹v’l‚ğŒ¸‚ç‚·
        if (Durability <= 0)
        {
            //‘Ï‹v’l‚ª0ˆÈ‰º‚É‚È‚Á‚½‚çƒAƒCƒeƒ€‚ğ”j‰ó‚·‚éˆ—
            Destroy(this.ItemPrefab);
        }
    }
    public override void Activation(float value, ItemRecieveData data)
    {
        Instantiate(ItemPrefab, data.pos, Quaternion.identity);
        //int AttackPos =data.pos;

        GameObject attack = Instantiate(AttackObject, data.pos, Quaternion.identity); //
        attack.transform.forward = data.dir; //
        attack.transform.rotation = Quaternion.identity; //
        //ˆê’èŠÔŒo‚Á‚½‚çAttackObject‚ğÁ‚·
        //if(AttackTime > 0)
        //{
  
        //}
        //Destroy(AttackObject);

        
    }
}
