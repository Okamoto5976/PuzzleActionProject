using System.Collections.Generic;
using UnityEngine;

//please check (wall-> renderer-> surface Type ->Transparent)

public class Transparent : MonoBehaviour
{
    [SerializeField] private Vector3Asset m_player;
    [SerializeField] private float m_fadeSpeed = 5f;
    [SerializeField] private MapGeneration m_mapGeneration;
    [SerializeField] private SpriteRenderer m_playerSpriteRenderer;

    private List<Renderer> m_allWalls = new();
    private HashSet<Renderer> m_currentHits = new();

    private Camera m_camera;

    private void Awake()
    {
        m_camera = GetComponent<Camera>();
    }

    private void Start()
    {
        if (m_mapGeneration == null)
        {
            Debug.LogWarning("Transparent : MapGeneration dose not exist");
        }

        foreach (GameObject wallObj in m_mapGeneration.SouthWall)
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

        float xRot = transform.rotation.eulerAngles.x;
        float fov = m_camera.fieldOfView;
        float nRot = xRot - (fov / 2);
        float cameraY = m_camera.transform.position.y;
        float tan = Mathf.Tan(nRot * Mathf.Deg2Rad);
        float cameraShadow = cameraY / tan;
        float cameraRatio = cameraY / cameraShadow;

        bool isWallInBetween = false;

        foreach (Renderer wall in m_allWalls)
        {
            if (wall == null) continue;
            if (!wall.isVisible) continue;

            float wallZ = wall.bounds.center.z;

            bool isBetween =
                wallZ >= minZ &&
                wallZ <= maxZ;

            if (!isBetween)
            {
                SetAlphaSmooth(wall, 1f);
                continue;
            }

            float distanceToWall = Mathf.Abs(cameraZ - wallZ);
            float remainingWallShadow = Mathf.Abs(cameraShadow - distanceToWall);
            float wallRatio = m_mapGeneration.WallScale.y / remainingWallShadow;
            if (wallRatio <= cameraRatio) continue;



            SetAlphaSmooth(wall, 0.3f);
            m_currentHits.Add(wall);
            isWallInBetween = true;

            foreach (Renderer otherWall in m_allWalls)
            {
                if (otherWall == null) continue;
                if (!wall.isVisible) continue;
                if (wall == otherWall) continue;

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

        if (isWallInBetween)
        {
            m_playerSpriteRenderer.sortingOrder = -1;
        } else
        {
            m_playerSpriteRenderer.sortingOrder = 1;
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