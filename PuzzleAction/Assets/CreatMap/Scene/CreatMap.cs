using System.Collections.Generic;
using UnityEngine;

public class CreatMap : MonoBehaviour
{
    [SerializeField] private MapClassData m_mapClassData;

    [SerializeField] private MapGeneration m_mapGenerate;
    [SerializeField] private EntitySpawner m_entitySpawner;

    [SerializeField] private DebugMapPlaceSystem m_debugMapPlaceSystem;
    [SerializeField] private InstanceCounter m_instanceCounter;

    private MapClass m_mapClass;
    private void Awake()
    {
        GameManager.Instance.ResetData();

        MapClass mapClass = m_mapClassData.MapClass;

        if (mapClass == null)
        {
            Debug.Log("DebugMapPlaceSystem On");

            m_instanceCounter.ResetCount();

            if (!m_debugMapPlaceSystem.DebugMapGenerate())
            {
                return;
            }
            else
            {
                m_mapClass = m_mapClassData.MapClass;
                if (m_mapClass != null) Debug.Log("MapClass in");
            }
        }

        m_mapGenerate.Generate(m_mapClassData);

        m_entitySpawner.Generate(m_mapClassData, m_mapGenerate);
    }
}

