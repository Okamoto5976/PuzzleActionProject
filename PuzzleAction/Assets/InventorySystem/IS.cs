//using Unity.VisualScripting;
//using UnityEngine;

//[System.Serializable]
//public class ItemBox
//{
//    public Data data;
//    public int count;

//    public ItemBox(Data data, int count)
//    {
//        this.data = data;
//        this.count = count;
//    }
//}

//public class IS : MonoBehaviour
//{
//    //アイテム移動は無くてもよい　＝＞　位置を変える必要はそこまでない　ホットバーに入れるのも中身を移動するものではなく参照を取るようにする
//    //アイテムにはパッシブとアクティブがある　アクティブは個数スタックで持てて　パッシブは一つのみ
//    //インベントリではアイテム追加アイテム削除
//    //配列にItemBox(中にDataとスタック数）を入れれるようにする
//    //ホットバーにはItemBoxのIndexを渡すことで　ホットバーのアイテムを使用＝＞インベントリで使用＝＞ホットバー＆インベントリから削除

//    //UI
//    //Image(button)のプレハブを作り　中にInventoryIcon（仮）を入れ　Indexが入るようにする
//    //buttonを押した際OnclickでIncentoryのindexをみて　ItemDataを取る　＜＝　ホットバー役のボタン
//    //buttonを押した際ホットバーに入れる＆アイテム削除を表示　＜＝　インベントリ役のボタン

   


//    // ==========Inventory================
//    public ItemBox[] slots = new ItemBox[30];

//    public bool AddItem(Data data, int count)
//    {
//        for(int i = 0; i < slots.Length;i++)//同じものがある場合スタック
//        {
//            if (slots[i] != null && slots[i].data == data)
//            {
//                slots[i].count += count;
//                return true;
//            }
//        }

//        for(int i = 0; i < slots.Length; i++)
//        {
//            if (slots[i]  == null)
//            {
//                slots[i] = new ItemBox(data, count);
//                return true;
//            }
//        }

//        return false;
//    }

//    //削除
//    public void RemoveItem(int index)
//    {
//        slots[index] = null;
//    }

//    //使用
//    public void UseItem(int index)
//    {
//        ItemBox item = slots[index];
//        if (item == null) return;

//        item.count--;
//        //ItemManagerに通知

//        if(item.count <= 0)
//        {
//            RemoveItem(index);
//            //ホットバーでも削除
//        }
//    }


//    //========hotber===========

//    public int[] hotbares = new int[3];

//    private void Awake()
//    {
//        for(int i = 0;i < hotbares.Length;i++)
//        {
//            hotbares[i] = -1;
//        }
//    }

//    //ホットバーに追加（設定）
//    public void AddHotber(int hotberNumber, int index)
//    {
//        hotbares[hotberNumber] = index;
//    }


//    //使用
//    public void Use(int hotberNumber)
//    {
//        int index = hotbares[hotberNumber];
//        if (index < 0)
//        {
//            return;
//        }

//        UseItem(index);
//    }


//    //インベントリ削除自クリア
//    public void HotberClear(int index)
//    {
//        for(int i = 0;  i < hotbares.Length;i++)
//        {
//            if(hotbares[i] == index)
//            {
//                hotbares[i] = -1;
//            }
//        }
//    }
//}
