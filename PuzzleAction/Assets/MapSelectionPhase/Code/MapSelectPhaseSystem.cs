using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MapDefinition
{
    public string m_name;

    public Vector2Int m_size;

    // マップの形
    public List<Vector2Int> m_activeTiles;
}

public class MapSelectPhaseSystem : MonoBehaviour
{
    [Header("Map Data")]
    [SerializeField] private MapClassData m_mapClassData;

    [Header("Map Definitions")]
    [SerializeField] private List<MapDefinition> m_maps;

    [Header("Build")]
    [SerializeField] private MapPlaceBuild m_build;

    [Header("Mouse")]
    [SerializeField] private Camera m_mainCamera;

    private List<GameObject> m_previews = new();

    private int m_selectedIndex = -1;

    private void Start()
    {
        GeneratePreviews();
    }

    private void GeneratePreviews()
    {
        float offset = 20f;

        for (int i = 0; i < m_maps.Count; i++)
        {
            var def = m_maps[i];

            GameObject preview =
                m_build.CreatePreviewMap(def, transform);

            preview.transform.position =
                new Vector3(i * offset, 0, 0);

            // collider（クリック用）
            var col = preview.AddComponent<BoxCollider>();
            col.size = new Vector3(def.m_size.x * 1.5f, 1, def.m_size.y * 1.5f);

            m_previews.Add(preview);
        }
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = m_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject obj = hit.collider.gameObject;

                int index = GetPreviewIndex(obj);

                if (index != -1)
                {
                    SelectMap(index);
                }
            }
        }

        // 決定
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            StartGame();
        }
    }

    private int GetPreviewIndex(GameObject obj)
    {
        for (int i = 0; i < m_previews.Count; i++)
        {
            if (obj.transform.IsChildOf(m_previews[i].transform) ||
                obj == m_previews[i])
            {
                return i;
            }
        }
        return -1;
    }

    private void SelectMap(int index)
    {
        m_selectedIndex = index;

        ApplyMap();

        Debug.Log($"Selected : {m_maps[index].m_name}");

        Highlight(index);
    }

    private void ApplyMap()
    {
        var def = m_maps[m_selectedIndex];

        MapClass map = new MapClass(def.m_size.x, def.m_size.y);

        foreach (var pos in def.m_activeTiles)
        {
            map.GetFloor(pos.x, pos.y)
               .SetState(Floor.FloorState.full);
        }

        m_mapClassData.SetMapClass(map);
    }

    private void Highlight(int index)
    {
        for (int i = 0; i < m_previews.Count; i++)
        {
            Color color = (i == index) ? Color.yellow : Color.white;

            foreach (var r in m_previews[i].GetComponentsInChildren<Renderer>())
            {
                r.material.color = color;
            }
        }
    }

    private void StartGame()
    {
        if (m_selectedIndex == -1)
        {
            Debug.Log("未選択");
            return;
        }

        SceneManager.LoadScene("MapPlaceSystem");
    }
}
