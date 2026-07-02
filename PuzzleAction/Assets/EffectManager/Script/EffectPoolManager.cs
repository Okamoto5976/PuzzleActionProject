using System.Collections.Generic;
using UnityEngine;

public class EffectPoolManager : MonoBehaviour
{
    private Queue<GameObject> m_pool = new();

    public GameObject Get(GameObject prefab)
    {
        if (m_pool.Count > 0)
        {
            GameObject obj = m_pool.Dequeue();

            obj.SetActive(true);

            return obj;
        }

        return Instantiate(prefab);
    }

    public void Return(GameObject obj)
    {
        ParticleSystem particle =
            obj.GetComponent<ParticleSystem>();

        if (particle != null)
        {
            particle.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        obj.SetActive(false);

        m_pool.Enqueue(obj);
    }
}