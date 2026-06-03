using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ItemManager : MonoBehaviour
{
    public enum EffectType
    {
        Null,
        Heal,
        Damage,
        Buff,
        Debuff,
    }
    //[SerializeField] private EntityData entityprefab;
    private ObjectPool<Entity> pool;
    public GameObject prefab; //アイテムのプレハブ
    //private int nextId = 1; //次のIDを管理する変数
    public struct ItemRecieveData
    {
        public Entity entity;
        public float baseValue; //Entity用　例）矢の攻撃力＋Entityの攻撃力
        public Vector3 pos;
        public Vector3 dir;//向き
        public Vector2 size;
    }

    //リスト初期化
    public List<Item> ItemList = new List<Item>();
 

    //Listの中からIDと同じアイテムを探す
    private Item LookForID(int id)
    {
        return ItemList.Find(x => x.Id == id);
    }
    public void ItemUse(int id , Entity entity)  // Entity
    {
        //見つけたアイテムを使用する}
        Item item = LookForID(id);
        if (item != null)
        {
         /*   item.RecieveData();*/
        }
        else
        {
            Debug.LogWarning($"ID{id}のアイテムは見つかりませんでした。");
        }

    }

    //ランダムにアイテムを渡す
    public Item RandomGetItem()
    {
        int index = Random.Range(0, ItemList.Count);
        return ItemList[index];

    }
    private  void  PrefabCool( ItemRecieveData data)
    {

        Debug.Log("Test");

        //ItemManagerがpoolからEntityを呼ぶItemManagerでpoolを仲介にenumで種類を渡す

        pool = new ObjectPool<Entity>(
        createFunc: () => Instantiate(prefab).GetComponent<Entity>(), //オブジェクトを生成する関数
        actionOnGet: entity => entity.gameObject.SetActive(true), //オブジェクトがプールから取得されるときに呼び出されるアクション
        actionOnRelease: entity => entity.gameObject.SetActive(false), //オブジェクトがプールから取得されるときに呼び出されるアクション
        actionOnDestroy: entity => Destroy(entity.gameObject), //オブジェクトがプールから削除されるときに呼び出されるアクション
        defaultCapacity: 10, //初期サイズ

            maxSize: 10); //ObjectPoolを初期化
        //Entity entity = EffectType.Count; //enumで種類を渡す



        //Playerから渡されたdataの中にある座標の位置に呼び出す
        GameObject obj = Instantiate(prefab, data.pos, Quaternion.LookRotation(data.dir)); //プレイヤーデータから座標と向きを呼び出す
        //Playerから渡されたdataのbaseValueを呼び出したEntityに渡す
        //Entity entity = ItemPool.entity(data.baseValue); //EntityにbaseValueを渡す
        
    }



}




//public void GetItemData(ItemData data) //座標やエンティティ自身
//{
//    switch (data.ItemType)
//    {
//        case ItemData.Itemtype.Value:
//            //Value type List
//            LookForID(data.ItemID, ValueList);
//            break;
//        case ItemData.Itemtype.Collider:
//            //Collider type List
//            LookForID(data.ItemID, ColliderList);
//            break;
//        case ItemData.Itemtype.Other:
//            //Other type List
//            LookForID(data.ItemID, OtherList);
//            break;
//        default:
//            break;
//    }