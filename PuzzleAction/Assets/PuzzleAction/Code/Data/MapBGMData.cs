using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MapBGMData", menuName = "Scriptable Objects/MapBGMData")]
public class MapBGMData : ScriptableObject
{
    [System.Serializable]
    public class MapBGM
    {
        public StageType m_type;

        public AudioData m_audioData;
    }

    public List<MapBGM> m_bgms = new();
}
