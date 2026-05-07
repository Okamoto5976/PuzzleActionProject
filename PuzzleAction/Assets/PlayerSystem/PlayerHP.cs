using UnityEngine;

public class PlayerHP : EntityHP
{
    protected override void Die()
    {
        Debug.Log("ゲームオーバー");
        //ゲームオーバー処理実行
        //StateをDie  動かせない＋アニメーション
    }
}
