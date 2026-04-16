using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "Scriptable Objects/EffectData")]
public class EffectData : ScriptableObject
{
    [SerializeField] private GameObject m_effectprefab;

    [SerializeField] private float m_duration;


    public GameObject EffectPrefab => m_effectprefab;
    public float Duration => m_duration;
}
