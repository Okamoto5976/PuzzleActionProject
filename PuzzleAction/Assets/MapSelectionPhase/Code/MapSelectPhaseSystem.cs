using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MapSelectPhaseSystem : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private MapClassData m_mapClassData;
    [SerializeField] private List<MapSettingSO> m_allMaps;
    [SerializeField] private int m_mapCount = 3;

    [SerializeField] private AudioData m_bgm;
    private Dictionary<int, MapRewardData> m_mapRewards = new();

    [Header("UI")]
    [SerializeField] private UnityEngine.UI.Button m_nextsceneButton;
    [SerializeField] private RectTransform m_previewRoot;
    [SerializeField] private Image m_tilePrefab;

    private readonly List<List<Image>> m_previewTiles = new();

    [SerializeField] private TMP_Text m_moneyTextPrefab;

    [SerializeField] private float m_maxPreviewWidth = 250f;
    [SerializeField] private float m_previewSpacing = 100f;

    [Header("Scene")]
    [SerializeField] private StaticSceneAsset m_mapPieceSystem;
    private readonly List<MapSettingSO> m_selectedMaps = new();
    private readonly List<RectTransform> m_previews = new();

    private float m_currentX;
    private int m_selectedIndex = -1;

    [SerializeField] private AudioData m_se;

    [Header("Tutorial")]
    [SerializeField] private MapSettingSO m_tutorialMap;
    [SerializeField] private GameObject m_guidePanel;

    private void Start()
    {
        m_nextsceneButton.onClick.AddListener(GoMapPieceSystem);
        AudioManager.Instance.PlayAudio(m_bgm);

        if(GameManager.Instance.IsTutorial)
        {
            m_guidePanel.SetActive(true);
            Tutorial();
            StartCoroutine(LoadManager.m_instance.FadeIn());

            return;
        }

        m_guidePanel.SetActive(false);


        CreateRandomMaps();
        GenerateRewards();
        CreatePreviews();

        StartCoroutine(LoadManager.m_instance.FadeIn());


    }

    private void Tutorial()
    {
        m_selectedMaps.Add(m_tutorialMap);

        m_mapRewards.Clear();
        m_mapRewards.Add(0, new MapRewardData(){StartMoney = 1000});

        CreatePreviews();
    }

    public void OnSE()
    {
        AudioManager.Instance.PlayAudio(m_se);
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
        grid.startCorner = GridLayoutGroup.Corner.LowerLeft;

        // -------- Generate Tiles --------

        List<Image> tiles = new();
        for (int y = 0; y < map.size.y; y++)
        {
            for (int x = 0; x < map.size.x; x++)
            {
                Image tile = Instantiate(m_tilePrefab, root);
                bool active = map.IsActiveTile(x, y);

                tile.color = active ? Color.white : Color.clear;

                tiles.Add(tile);
            }
        }

        m_previewTiles.Add(tiles);
        m_previews.Add(root);

        GameObject rewardObj = new GameObject("RewardText");

        rewardObj.transform.SetParent(root, false);
        TMP_Text moneyText = Instantiate(m_moneyTextPrefab, rewardObj.transform);
        moneyText.text = $" + ¥{m_mapRewards[index].StartMoney}";
        moneyText.alignment = TextAlignmentOptions.Center;

        RectTransform textRect = moneyText.rectTransform;
        textRect.anchorMin = new Vector2(0.5f, 1f);
        textRect.anchorMax = new Vector2(0.5f, 1f);
        textRect.pivot = new Vector2(0.5f, 0f);
        textRect.anchoredPosition = new Vector2(0, 260f);
    }
    private void GenerateRewards()
    {
        //reset 
        m_mapRewards.Clear();
        for (int i = 0; i < m_selectedMaps.Count; i++)
        {
            MapRewardData reward = new();
            reward.StartMoney = Random.Range(500, 1500);
            m_mapRewards.Add(i, reward);
        }
    }
    #endregion

    #region Select
    private void Update()
    {
        if (m_selectedIndex < 0) return;

        float alpha = Mathf.Lerp(0.6f, 1.0f, (Mathf.Sin(Time.time * 4f) + 1f) * 0.5f);
        UpdateSelectedAlpha(alpha);
    }

    private void SelectMap(int index)
    {
        //Debug.Log($"SelectMap : {index}");
        OnSE();

        m_selectedIndex = index;
        ApplyMap();
        Highlight(index);
        if (m_selectedMaps[index].mapName == null) return;
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

            m_previews[i].localScale = selected ? Vector3.one * 1.1f : Vector3.one;

            foreach (Image tile in m_previewTiles[i])
            {
                if (tile == null) continue;

                if (tile.color.a <= 0f) continue;

                Color color = tile.color;
                color.a = selected ? 1f : 0.4f;
                tile.color = color;
            }
        }
    }

    private void UpdateSelectedAlpha(float alpha)
    {
        if (m_selectedIndex < 0) return;

        if (m_selectedIndex >= m_previewTiles.Count) return;

        foreach (Image tile in m_previewTiles[m_selectedIndex])
        {
            if (tile == null) continue;
            if (tile.color.a <= 0f) continue;

            Color color = tile.color;
            color.a = alpha;
            tile.color = color;
        }
    }
    #endregion
    #region ApplyMap

    private void ApplyMap()
    {
        var definition = m_selectedMaps[m_selectedIndex];

        MapClass map = new MapClass(definition.size.x, definition.size.y);
        for (int y = 0; y < definition.size.y; y++)
        {
            for (int x = 0; x < definition.size.x; x++)
            {
                map.GetFloor(x, y).SetState(Floor.FloorState.blocked);
            }
        }

        for (int y = 0; y < definition.size.y; y++)
        {
            for (int x = 0; x < definition.size.x; x++)
            {
                if (!definition.IsActiveTile(x, y)) continue;

                map.GetFloor(x, y).SetState(Floor.FloorState.empty);
            }
        }
        map.UpdateFloors();

        m_mapClassData.SetMapClass(map);
        m_mapClassData.SetStartPos(definition.startPos);
        m_mapClassData.SetGoalPos(definition.goalPos);
        m_mapClassData.SetRewardData(m_mapRewards[m_selectedIndex]);
    }

    #endregion

    #region SceneMove

    public void GoMapPieceSystem()
    {
        if (m_selectedIndex == -1)
        {
            return;
        }
        LoadManager.m_instance.LoadScene(m_mapPieceSystem.Value);
    }

    #endregion
}