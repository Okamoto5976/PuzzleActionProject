using UnityEngine;

public struct RockUseData 
{
    //使用者
    public GameObject Owner;

    //出現位置
    public Vector3 Position;

    //飛ぶ方向
    public Vector3 Direction;

    //範囲
    public float Range;

    //public static implicit operator TrapUseData(RockUseData rus)
    //{
    //    var tud = new TrapUseData();
    //    tud.Owner = rus.Owner;
    //    tud.Position = rus.Position;
    //    tud.Direction = rus.Direction;
    //    return tud;
    //}
}
