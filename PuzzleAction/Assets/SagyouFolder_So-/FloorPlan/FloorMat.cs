using UnityEngine;
using System.Collections.Generic;

public class FloorMat : MonoBehaviour
{
    [SerializeField] private List<Texture> m_textures = new();

    private Renderer m_ren;
    private void Awake()
    {
        m_ren = GetComponent<Renderer>();
    }

    private void Start()
    {
        int length = m_textures.Count;

        int index = UnityEngine.Random.Range(0, length);

        Texture texture = m_textures[index];

        m_ren.material.SetTexture("_BaseMap", texture);
    }
}
