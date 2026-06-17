using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    // private Light m_light;

    [Header("Light")]
    [SerializeField] private CustomPlayerLight m_playerLightController;

    [Header("Brightness")]
    [Range(0f, 20f)]
    [SerializeField] private float m_intensity = 5;

    [Header("Distance")]
    [Range(1f, 50f)]
    [SerializeField] private float m_range = 10f;

    [Header("Light Spread")]
    [Range(1f, 179f)]
    [SerializeField] private float m_IrradiationRange = 60;

    [Header("Position Offset")]
    [SerializeField]private Vector3 m_lightPositionOffset = new Vector3(0f, 2f, 0f);

    private float m_currentIntensity;
    private float m_cuttentRange;
    private float m_cuttentSpotAngle;

    private void OnValidate()
    {
        if (m_playerLightController == null) return;

        m_playerLightController.SetLightParameters(m_intensity, m_range, m_IrradiationRange, m_lightPositionOffset);
    }

    //private void Awake()
    //{
    //   m_light = GetComponent<Light>();
    //}

    //private void Start()
    //{

    //   m_light.intensity = m_intensity;

    //}


    public void SetIntensity()
    {

    }
}
