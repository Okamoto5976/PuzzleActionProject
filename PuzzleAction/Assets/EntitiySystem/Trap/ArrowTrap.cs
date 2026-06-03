using UnityEngine;

public class ArrowTrap : TrapBase
{
    [Header("ArrowData")]
    [SerializeField]
    private TrapData m_trapdata;

    public void Initialize(TrapUseData data)
    {
        //position
        transform.position = data.Position;

        //Normalizedirection
        m_direction = data.Direction.normalized;

        //startposition
        m_startPosition = transform.position;

        //user
        m_owner = data.Owner;

        //BaseValue
        //m_basevalue = data.BaseValue;

        //Data
        m_range=m_trapdata.range;

        //damage
        m_damage = m_trapdata.damage + m_basevalue;
    }     
}
