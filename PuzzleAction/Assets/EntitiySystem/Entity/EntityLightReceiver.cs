using UnityEngine;

public class EntityLightReceiver : MonoBehaviour
{
    [SerializeField] private Light m_testLight;
    [SerializeField] private Renderer m_renderer;

    private MaterialPropertyBlock m_propertyBlock;

    private void Awake()
    {
        m_propertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        if (m_testLight == null)
            return;

        // ŒõŒ¹ ¨ Entity ‚Ì•ûŒü
        Vector3 lightDirection =
            (transform.position - m_testLight.transform.position).normalized;

        // ŒõŒ¹‚Ü‚Å‚Ì‹——£
        float distance =
            Vector3.Distance(transform.position, m_testLight.transform.position);

        // Light‚Ì”ÍˆÍ‚É‚æ‚éŒ¸Š
        float distanceFactor =
            1f - Mathf.Clamp01(distance / m_testLight.range);

        // ShaderGraph‚Ö“n‚·
        m_renderer.GetPropertyBlock(m_propertyBlock);

        m_propertyBlock.SetVector(
            "_DirectionLight",
            lightDirection
        );

        m_propertyBlock.SetColor(
            "_LightColor",
            m_testLight.color
        );

        m_propertyBlock.SetFloat(
            "_Intensity",
            m_testLight.intensity * distanceFactor
        );

        m_renderer.SetPropertyBlock(m_propertyBlock);
    }
}
