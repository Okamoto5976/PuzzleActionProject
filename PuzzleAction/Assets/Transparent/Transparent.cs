using System.Collections.Generic;
using UnityEngine;

//please check (wall-> renderer-> surface Type ->Transparent)

public class Transparent : MonoBehaviour
{
    [SerializeField] private Vector3Asset m_player;
    [SerializeField] private float m_fadeSpeed = 5f;
    [SerializeField] private CreatMap m_createMap;

    private List<Renderer> m_allWalls = new();
    private HashSet<Renderer> m_currentHits = new();

    private Camera m_camera;

    private void Awake()
    {
        m_camera = GetComponent<Camera>();
    }

    private void Start()
    {
        if (m_createMap == null) return;

        foreach (GameObject wallObj in m_createMap.SouthWall)
        {
            if (wallObj == null) continue;

            Renderer renderer = wallObj.GetComponent<Renderer>();

            if (renderer == null)
            {
                continue;
            }

            m_allWalls.Add(renderer);
        }
    }

    private void Update()
    {
        m_currentHits.Clear();

        float cameraZ = m_camera.transform.position.z;
        float playerZ = m_player.Value.z;

        float minZ = Mathf.Min(cameraZ, playerZ);
        float maxZ = Mathf.Max(cameraZ, playerZ);

        foreach (Renderer wall in m_allWalls)
        {
            if (wall == null) continue;

            float wallZ = wall.bounds.center.z;

            bool isBetween =
                wallZ >= minZ &&
                wallZ <= maxZ;

            if (!isBetween)
            {
                SetAlphaSmooth(wall, 1f);
                continue;
            }

            SetAlphaSmooth(wall, 0.3f);
            m_currentHits.Add(wall);

            foreach (Renderer otherWall in m_allWalls)
            {
                if (otherWall == null) continue;

                float otherZ = otherWall.bounds.center.z;

                if (Mathf.Abs(otherZ - wallZ) < 0.1f)
                {
                    SetAlphaSmooth(otherWall, 0.3f);
                    m_currentHits.Add(otherWall);
                }
            }

        }
        foreach (Renderer wall in m_allWalls)
        {
            if (wall == null) continue;

            if (!m_currentHits.Contains(wall))
            {
                SetAlphaSmooth(wall, 1f);
            }
        }
    }

    private void SetAlphaSmooth(Renderer renderer, float targetAlpha)
    {
        foreach (Material mat in renderer.materials)
        {
            Color color = mat.color;
            color.a = Mathf.Lerp(color.a,targetAlpha,Time.deltaTime * m_fadeSpeed);

            mat.color = color;
        }
    }
}