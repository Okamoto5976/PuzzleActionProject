using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    
    Item item; // アイテムのクラス
    dropPool dropList; // ドロップするアイテムのリストのクラス
    dropPool pool; // アイテムを管理するドロッププールのクラス
    //public event Action m_event;
    void dropItem(Vector3 position) // アイテムをドロップするメソッド
    {
        //if (Random.value<= dropList.DropRate) // ドロップ率に基づいてアイテムをドロップするかどうかを決定
        //{
        //    pool.GetItem(position); // アイテムを取得してドロップ
        //}
    }


    //playerの座標か
    //自身の半径３mいないに　プレイヤーが入ったら　プレイヤーにアイテムを渡す。
    private void ItemGet(Collider othor)
    {
        if (othor.CompareTag("Player"))
        {
            Return();
        }
    }

    public void Initialize()
    {
        Invoke(nameof(Return), 5f); //5秒後にReturnメソッドを呼び出す
    }
    private void Return()
    {
        if (pool != null)
        {

            //Poolに返す処理
            pool.ReturnItem(gameObject);
        }
    }
}
