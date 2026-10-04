using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    [SerializeField]
    private Middleman_Effect m_effectPool;

    public void PlayParticle(Enum_EffectType type, Vector3 pos)
    {
        EffectObj obj = m_effectPool.GetComponent(type);

        obj.transform.position = pos;

        obj.gameObject.SetActive(true);

        ParticleSystem particle =
            obj.GetComponentInChildren<ParticleSystem>();

        if (particle != null)
        {
            particle.Clear();
            particle.Play();
        }

        obj.Initialize();

    }

}