using UnityEngine;

[RequireComponent (typeof(Light))]
public class CustomPlayerLight : MonoBehaviour
{
    private Light m_light;

    private void SetupLight()
    {
        if (m_light == null)
        {
            m_light = GetComponent<Light>();
        }
    }

    public void SetLightParameters(float intensity, float range, float irradiationRange, Vector3 position)
    {
        SetupLight();

        if (m_light == null) return;

        m_light.intensity = intensity;
        m_light.range = range;
        m_light.spotAngle = irradiationRange;

        transform.localPosition = position;
    }
}
