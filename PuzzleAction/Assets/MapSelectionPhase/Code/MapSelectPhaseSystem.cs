using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelectPhaseSystem : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private MapClassData m_mapClassData;
    [SerializeField] private List<MapSettingSO> m_allMaps;
    [SerializeField] private int m_mapCount = 3;

    [Header("UI")]
    [SerializeField] private RectTransform m_previewRoot;
    [SerializeField] private Image m_tilePrefab;

    [SerializeField] private float m_maxPreviewWidth = 250f;
    [SerializeField] private float m_previewSpacing = 100f;

    private readonly List<MapSettingSO> m_selectedMaps = new();
    private readonly List<RectTransform> m_previews = new();

    private float m_currentX;
    private int m_selectedIndex = -1;

    private void Start()
    {
        CreateRandomMaps();
        CreatePreviews();
    }

    #region Create Maps

    private void CreateRandomMaps()
    {
        List<MapSettingSO> copy = new(m_allMaps);

        int count = Mathf.Min(m_mapCount, copy.Count);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, copy.Count);

            m_selectedMaps.Add(copy[index]);

            copy.RemoveAt(index);
        }
    }

    private void CreatePreviews()
    {
        float totalWidth = 0f;

        foreach (var map in m_selectedMaps)
        {
            float cellSize = Mathf.Min(m_maxPreviewWidth / map.size.x, m_maxPreviewWidth / map.size.y);

            totalWidth += map.size.x * cellSize;
        }

        totalWidth += (m_selectedMaps.Count - 1) * m_previewSpacing;

        m_currentX = -totalWidth * 0.5f;

        for (int i = 0; i < m_selectedMaps.Count; i++)
        {
            CreatePreview(i);
        }
    }

    private void CreatePreview(int index)
    {
        MapSettingSO map = m_selectedMaps[index];

        float cellSize = Mathf.Min(m_maxPreviewWidth / map.size.x, m_maxPreviewWidth / map.size.y);

        float width = map.size.x * cellSize;
        float height = map.size.y * cellSize;

        GameObject rootObj = new GameObject($"MapPreview_{index}");
        rootObj.transform.SetParent(m_previewRoot, false);

        RectTransform root = rootObj.AddComponent<RectTransform>();
        root.sizeDelta = new Vector2(width, height);
        root.anchoredPosition = new Vector2(m_currentX + width * 0.5f, 0);

        m_currentX += width + m_previewSpacing;

        // -------- Background --------

        Image background = rootObj.AddComponent<Image>();

        background.color = new Color(0f, 0f, 0f, 0f);

        // -------- Button --------

        UnityEngine.UI.Button button = rootObj.AddComponent<UnityEngine.UI.Button>();
        int capturedIndex = index;

        button.onClick.AddListener(() =>{SelectMap(capturedIndex);});

        // -------- Grid --------

        GridLayoutGroup grid = rootObj.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(cellSize, cellSize);
        grid.spacing = Vector2.zero;

        // -------- Generate Tiles --------

        for (int y = 0; y < map.size.y; y++)
        {
            for (int x = 0; x < map.size.x; x++)
            {
                Image tile = Instantiate(m_tilePrefab, root);

                bool active = map.IsActiveTile(x, y);

                tile.color =    
                    active
                    ? Color.white
                    : Color.clear;
            }
        }

        m_previews.Add(root);
    }

    #endregion

    #region Select

    private void SelectMap(int index)
    {
        Debug.Log($"SelectMap : {index}");

        m_selectedIndex = index;

        ApplyMap();

        Highlight(index);

        Debug.Log($"Selected : {m_selectedMaps[index].mapName}");
    }

    public void Deselect()
    {
        m_selectedIndex = -1;

        Highlight(-1);
    }

    private void Highlight(int index)
    {
        for (int i = 0; i < m_previews.Count; i++)
        {
            bool selected = i == index;

            //­‚µ‘å‚«‚­‚·‚é
            m_previews[i].localScale =
                selected
                ? Vector3.one * 1.1f
                : Vector3.one;
        }
    }

    #endregion

    #region ApplyMap

    private void ApplyMap()
    {
        var definition = m_selectedMaps[m_selectedIndex];

        MapClass map = new MapClass(definition.size.x, definition.size.y);

        // ‘S•” Blocked

        for (int y = 0; y < definition.size.y; y++)
        {
            for (int x = 0; x < definition.size.x; x++)
            {
                map.GetFloor(x, y).SetState(Floor.FloorState.blocked);
            }
        }

        // Shape‚Ì1‚¾‚¯—LŒø

        for (int y = 0; y < definition.size.y; y++)
        {
            for (int x = 0; x < definition.size.x; x++)
            {
                if (!definition.IsActiveTile(x, y))
                    continue;

                map.GetFloor(x, y).SetState(Floor.FloorState.empty);
            }
        }

        map.UpdateFloors();

        m_mapClassData.SetMapClass(map);
        m_mapClassData.SetStartPos(definition.startPos);
        m_mapClassData.SetGoalPos(definition.goalPos);
    }

    #endregion

    #region SceneMove

    public void GoMapPieceSystem()
    {
        if (m_selectedIndex == -1)
        {
            Debug.Log("Map Not Selected");
            return;
        }

        SceneManager.LoadScene("MapPieceSystem");
    }

    #endregion
}