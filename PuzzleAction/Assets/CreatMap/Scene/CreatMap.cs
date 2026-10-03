using System.Collections.Generic;
using UnityEngine;

public class CreatMap : MonoBehaviour
{
    [SerializeField] private MapClassData m_mapClassData;

    [SerializeField] private MapGeneration m_mapGenerate;
    [SerializeField] private EntitySpawner m_entitySpawner;

    [SerializeField] private DebugMapPlaceSystem m_debugMapPlaceSystem;
    [SerializeField] private InstanceCounter m_instanceCounter;

    [SerializeField] private InventorySystem m_inventorySystem;
    [SerializeField] private GameObject m_player;

    private MapClass m_mapClass;

    [SerializeField] private MapBGMData m_mapBGMData;
    [SerializeField] private EnemyCameraDetector m_enemyCameraDetector;

    private void Awake()
    {
        GameManager.Instance.ResetData();

        MapClass mapClass = m_mapClassData.MapClass;

        if (mapClass == null)
        {
            //Debug.Log("DebugMapPlaceSystem On");

            m_instanceCounter.ResetCount();

            if (!m_debugMapPlaceSystem.DebugMapGenerate())
            {
                return;
            }
            else
            {
                m_mapClass = m_mapClassData.MapClass;
                //if (m_mapClass != null) Debug.Log("MapClass in");
            }
        }

        m_mapGenerate.Generate(m_mapClassData);
        m_entitySpawner.Generate(m_mapClassData, m_mapGenerate);
    }

    private void Start()
    {
        m_inventorySystem.Initialized();
        m_entitySpawner.SpawnTitleTrophy(m_player.transform.position);

        PlayBGM();
    }


    private void PlayBGM()
    {
        var list = m_mapBGMData.m_bgms;

        AudioManager.Instance.PlayAudio(list[0].m_audioData);

        m_enemyCameraDetector.SetMapBGM(list[0].m_audioData);

    }
}

