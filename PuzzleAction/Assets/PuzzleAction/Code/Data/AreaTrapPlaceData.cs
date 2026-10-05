using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AreaPlacementTrapData", menuName = "Scriptable Objects/Datas/AreaPlacementTrapData")]
public class AreaTrapPlaceData : ScriptableObject
{
    [System.Serializable]
    public class AreaTrapPlaceClass
    { 
        public Enum_TrapType m_trapType;
        public float m_num;
    }

    [SerializeField] private List<AreaTrapPlaceClass> m_areaTrapPlaceList = new();

    public List<AreaTrapPlaceClass> AreaTrapPlaceList => m_areaTrapPlaceList;

    public float GetAreaTrapPlaceData(Enum_TrapType type)
    {
        return m_areaTrapPlaceList.Find(x => x != null && x.m_trapType == type).m_num;
    }
}
