using UnityEngine;
using UnityEngine.Pool;

public class ItemDrop : MonoBehaviour
{
    private dropPool pool;
    //public event Action m_event;

    

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
