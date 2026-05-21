using UnityEngine;
using UnityEngine.UI;

public class TestItemData
{
    public string name;
    public int price;
    public string info;
    public Image icon;
}

public class ShopManager : MonoBehaviour
{
    //ItemManagerからItemDataをもらう

    private void Awake()
    {
        //仮でItemData生成　本来ItemManagerから取得
        TestItemData data = new TestItemData(

            );
    }


    //[SerializeField] private GameObject

    //配置をコードで行う
    //座標をListに入れる

    //もらったItemData(仮で６つ）
    //prefabを生成、ItemDataを渡し、prefabを座標をもとに配置（forで6回）

    //clickで購入
    //値段とお金を比較　購入ならインベントリに通知
    //もし購入時にSold と表示（購入済み）するなら　boolを渡す
    //
}
