using UnityEngine;

//バフアイコン
public enum BuffID
{
    None,//ステータスを減少させるとき（バフアイコンにはならない）
    STR_I,
    STR_II,
    STR_III,
    Speed_I,
    Speed_II,
    Speed_III,
    DEF_I,
    DEF_II,
    DEF_III,
    CR_I,
    CR_II,
    CR_III,
    CD_I,
    CD_II,
    CD_III,
    BR_I,
    BR_II,
    Regenerate,
    STRDown,
    SpeedDown,
    DEFDown,
    CRDown,
    CDDown,
    //baffDamageは　種類を問わず更新する　値の高いものが優先
    Poison,
    Water,
    Gas,
    Burn,
    Swamp,
    Stun,
    Invincible
}
