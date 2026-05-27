using UnityEngine;

public class ArrowTrap : TrapBase
{
    [Header("ArrowData")]
    [SerializeField]
    private ArrowData m_arrowdata;

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
        m_range=m_arrowdata.range;

        m_damage = m_arrowdata.damage;
    }     
}
