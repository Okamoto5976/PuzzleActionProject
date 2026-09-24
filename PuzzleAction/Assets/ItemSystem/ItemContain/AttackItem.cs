using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AttackItem", menuName = "Scriptable Objects/Datas/AttackItem")]
public class AttackItem : Item
{
    public  enum AttackItemType
    {
        Shot,
        Club,
        Sword,
        Chopsticks
    }

    //[SerializeField] private ItemData ItemName;
    //[SerializeField] private float specialEffectDuration; //特殊効果時間

    [SerializeField] private DamageData damage;
    [SerializeField] private AttackItemType m_attackType;

    [System.Serializable]
    public class BuffModify
    {
        public StatusModifier m_statusmod;
        public BuffID m_buffID;
        public float m_duration;
    }

    [SerializeField] private List<BuffModify> m_buffList = new();

    public override void Activation(ItemRecieveData data)
    {
        //powerで当たり判定を大きく

        //設定
        Collider[] hits = Physics.OverlapSphere(
            data.pos,
            3f
            );

        //Pos
        //座標の指定

        foreach (Collider hit in hits)
        {
            Entity entity = hit.GetComponentInParent<Entity>();
            if (entity != null)
            {
                continue;
            }

            //Effect
            //効果（ダメージや）
            switch (m_attackType)
            {
                case AttackItemType.Shot:
                    

                    
                    break;

                case AttackItemType.Club:
                    break;

                case AttackItemType.Sword:
                    break;

                case AttackItemType.Chopsticks:

                    break;
            }
            entity.TakeDamage(damage);
        }


        Debug.Log($"AttackItemを使用しました");
    }

}
