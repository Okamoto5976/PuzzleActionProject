using UnityEngine;

[System.Serializable]
public struct EffectData
{
    public GameObject m_effectPrefab;
    public Vector3 m_effectPos;
    public Quaternion m_effectRot;

    public float m_lifeTime;
}
