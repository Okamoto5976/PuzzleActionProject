using UnityEngine;

public class RockTrap : TrapBase
{
    [Header("RockData")]
    [SerializeField]
    private RockData m_rockdata;

    public void Initialize(TrapUseData data)
    {
        //出現位置
        transform.position = data.Position;

        //Normalize方向(正規化)
        m_direction = data.Direction.normalized;

        //初期位置保存
        m_startPosition = transform.position;

        //使用者
        m_owner = data.Owner;

        //Data
        m_range=m_rockdata.range;

        m_damage = m_rockdata.damage;
    }
}
