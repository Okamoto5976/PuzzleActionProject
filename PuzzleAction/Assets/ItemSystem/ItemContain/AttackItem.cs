using UnityEngine;


public  enum AttackItemType
{
    Sword,
    Shot,
    Rod
}


[CreateAssetMenu(fileName = "AttackItem", menuName = "Scriptable Objects/Datas/AttackItem")]
public class AttackItem : Item
{

    //[SerializeField] private ItemData ItemName;
    //[SerializeField] private float specialEffectDuration; //特殊効果時間

    [SerializeField] private DamageData damage;
    [SerializeField] private AttackItemType a_type;
    [SerializeField] private Transform i_transform;

    public override void Activation(ItemRecieveData data)
    {
        
        //Collider
        
        //設定
        Collider[] hits = Physics.OverlapSphere(
            i_transform.position,
            data.power 
            );

        //Pos
        //座標の指定
        
        hits[1].transform.position = data.pos;

        foreach (Collider hit in hits)
        {
            Entity entity = hit.GetComponentInParent<Entity>();
            if (entity != null)
            {
                continue;
            }

            //Effect
            //効果（ダメージや）
            switch (a_type)
            {
                case AttackItemType.Sword:
                    break;

                case AttackItemType.Shot:
                    break;

                case AttackItemType.Rod:
                    break;
            }
            entity.TakeDamage(damage);
        }


        Debug.Log($"AttackItemを使用しました");
    }

}
