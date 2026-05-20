using UnityEngine;

public class WeaponItem : Item
{

    public enum WeponType
    {
        Short,//近距離
        Long,//遠距離
        Medium, //中距離
        
    }
    [SerializeField] private float WeponDameg;
    [SerializeField] private float WeponRange;
    [SerializeField] private WeponType weponType;
    [SerializeField] private GameObject EffectPrefab; //攻撃エフェクトのプレハブ
    [SerializeField] private float Durability; //耐久値

    void Short()
    {
        //近距離の攻撃処理


    }
    void Bow()
    {
        //遠距離の攻撃処理

    }
    void Medium()
    {
        //中距離の攻撃処理
    }
    void Durabity(float durability)
    {
        //耐久値の処理
        Durability -= 1; //攻撃するたびに耐久値を減らす
        if (Durability <= 0)
        {
            //耐久値が0以下になったらアイテムを破壊する処理
            //例: Destroy(this.gameObject);
        }
    }
    public override void Activation(float value, ItemRecieveData data)
    {
        //Collider[] hitColliders = Physics.OverlapSphere(transform.position, WeponRange); //攻撃範囲内のコライダーを取得
        if (weponType == WeponType.Short)//近距離
        {
            Short();
        }
        else if (weponType == WeponType.Long)//遠距離
        {
            Bow();
        }
        else if (weponType == WeponType.Medium)//中距離
        {
            Medium();
        }
        
        if(data.entity.TryGetComponent<ColliderHit>(out ColliderHit hit))
        {
            //hit.Dameg = WeponDameg;
            //hit.HitEffect = EffectPrefab;
            Durabity(Durability);
        }

    }
}
