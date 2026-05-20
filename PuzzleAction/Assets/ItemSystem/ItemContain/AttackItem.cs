using UnityEngine;
[CreateAssetMenu(fileName = "AttackItem", menuName = "Scriptable Objects/Datas/AttackItem")]
public class AttackItem : Item
{

    [SerializeField] private float attackRange; //UŒ‚”ÍˆÍ
    [SerializeField] private float Damage; //UŒ‚—Í
    public override void Activation(float value, ItemRecieveData data)
    {
        //        Entity entity = data.entity;   
        //if (entity == null) return;
        //Vector3 dir = data.dir;
        //Vector3 pos = data.pos;
        //Collider[] hitColliders = Physics.OverlapSphere(pos, attackRange);
        //foreach (var hitCollider in hitColliders)
        //{
        //    Entity targetEntity = hitCollider.GetComponent<Entity>();
        //    if (targetEntity != null && targetEntity != entity)
        //    {
        //        // ƒ_ƒ[ƒW‚ğ—^‚¦‚éˆ—
        //        //targetEntity.BuffSet(BuffItem.BuffType.Damage, Damage, 0);
        //    }
    }
}