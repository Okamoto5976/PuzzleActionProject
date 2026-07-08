using UnityEngine;

public class LightController : MonoBehaviour
{
    [Header("Player Light")]
    public Light m_playerLight;

    [Range(0f, 10f)]
    public float m_intensity = 3f;

    [Range(1f, 30f)]
    public float m_range = 10f;

    public Color m_lightColor = Color.white;

    private void Update()
    {
        m_playerLight.intensity = m_intensity;
        m_playerLight.range = m_range;
        m_playerLight.color = m_lightColor;
    }
}
