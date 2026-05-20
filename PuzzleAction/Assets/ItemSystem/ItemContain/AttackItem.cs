using JetBrains.Annotations;
using UnityEngine;
[CreateAssetMenu(fileName = "AttackItem", menuName = "Scriptable Objects/Datas/AttackItem")]
public class AttackItem : Item
{
    [SerializeField] private float attackRange; //攻撃範囲
    [SerializeField] private float Damage; //攻撃力
    public ItemPool itempool; //アイテムプールへの参照


    public override void Activation(float value, ItemRecieveData data)
    {

        //ItemPool pool　= itempool; //アイテムプールから攻撃エフェクトを取得

        //Entity entity = pool.Get(ItemManager.EffectType.); //攻撃エフェクトのタイプを指定して取得
        //if (entity != null) {
        //    entity.transform.position = data.pos; //攻撃エフェクトの位置を設定
        //    entity.transform.rotation = Quaternion.LookRotation(data.dir); //攻撃エフェクトの向きを設定
        //    entity.ApplyEffect(Damage); //攻撃エフェクトにダメージを適用
        //}

        base.Activation(value, data);
        //攻撃処理
        Collider[] hitColliders = Physics.OverlapSphere(data.pos, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            //攻撃対象にダメージを与える処理
            //例: hitCollider.GetComponent<Health>()?.TakeDamage(Damage);
        }
          

    }
}