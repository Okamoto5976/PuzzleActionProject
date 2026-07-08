using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "Scriptable Objects/EffectData")]
public class EffectData : ScriptableObject
{
    [SerializeField] private GameObject m_effectprefab;

    [SerializeField] private float m_duration;

    [SerializeField] private Enum_EffectType m_type;

    public GameObject EffectPrefab => m_effectprefab;
    public float Duration => m_duration;
    public Enum_EffectType Type => m_type;
}
