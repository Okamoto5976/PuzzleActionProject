using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class MapSelectPhaseSystem : MonoBehaviour
{
    [Header("ref")]
    [SerializeField] private InputActionReference m_action;
    [SerializeField] private MapClassData m_mapClassData;
    [Header("map")]
    [SerializeField] private int m_mapCount = 3;
    [SerializeField] private List<MapSettingSO> m_allMaps;
    [SerializeField] private GameObject m_cube;

    [SerializeField] private Camera m_camera;
    private Vector3 m_mouseWorldPos;

    private List<MapSettingSO> m_selectedMaps = new();
    private List<GameObject> m_mapParents = new();

    private int m_selectedIndex = -1;

    [SerializeField] private IntRunTime m_levelSO;


    private void Start()
    {
        CreatRandomMaps();
        GenerateMaps();
    }

    private void CreatRandomMaps()
    {
        //List<MapSettingSO> copy = new(m_allMaps);

        for(int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, m_allMaps.Count);
            m_selectedMaps.Add(m_allMaps[index]);
            m_allMaps.RemoveAt(index);
        }
    }

    private void GenerateMaps()
    {
        for(int i = 0;i < m_mapCount; i++)
        {
            var definition = m_selectedMaps[i];

            //parent
            GameObject parent = new GameObject($"Map_{i}");
            parent.transform.position = new Vector3(i * 20, 0, 0);

            //child
            foreach(var pos in definition.activeTiles)
            {
                GameObject child = Instantiate(m_cube, parent.transform);

                child.transform.localPosition =
                    new Vector3(
                    (pos.x + 0.5f) * 1.5f,
                    0,
                    (pos.y + 0.5f) * 1.5f);
            }

            //collider
            BoxCollider collider = parent.AddComponent<BoxCollider>();
            collider.size = new Vector3(
                definition.size.x * 1.5f,
                1,
                definition.size.y * 1.5f);

            collider.center = new Vector3(
                (definition.size.x * 1.5f) / 2,
                0,
                (definition.size.y * 1.5f) / 2);

        m_mapParents.Add(parent);
        }   
    }

    private void Update()
    {
        #region operation mouse
        if (m_action.action.WasPressedThisFrame())
        {
            Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
            Ray ray = m_camera.ScreenPointToRay(mouseScreenPos);
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                SelectFormatHit(hit.collider.gameObject);
            }
        }
        #endregion
    }

    private void SelectFormatHit(GameObject obj)
    {
        for(int i = 0;i < m_mapParents.Count;i++)
        {
            if (obj.transform.IsChildOf(m_mapParents[i].transform) ||
                obj == m_mapParents[i])
            {
                SelectMap(i);
                return;
            }
        }
    }

    #region Select System
    /// <summary>
    /// select System
    /// </summary>
    /// <param name="index"></param>
    private void SelectMap(int index)
    {
        m_selectedIndex = index;

        ApplyMap();

        Highlight(index);

        Debug.Log($"Select: {m_selectedMaps[index].name}");
    }
    #endregion

    #region Apply MapClassData
    /// <summary>
    /// apply MapClassData
    /// </summary>
    public void ApplyMap()
    {
        var definition = m_selectedMaps[m_selectedIndex];

        Debug.Log(definition);
        MapClass map = new MapClass(definition.size.x, definition.size.y);

        HashSet<Vector2Int> activeSet = new HashSet<Vector2Int>(definition.activeTiles);

        for (int y = 0; y < definition.size.y; y++)
        {
            for (int x = 0; x < definition.size.x; x++)
            {
                map.GetFloor(x, y).SetState(Floor.FloorState.blocked);
            }
        }

        foreach (var pos in definition.activeTiles)
        {
            map.GetFloor(pos.x, pos.y).SetState(Floor.FloorState.empty);
        }

        m_mapClassData.SetGoalPos(definition.goalPos);
        m_mapClassData.SetStartPos(definition.startPos);
        map.UpdateFloors();
        m_mapClassData.SetMapClass(map);
    }
    #endregion

    //--------------------------Debug------------------------
    private void Highlight(int index)
    {
        for(int i = 0; i < m_mapParents.Count;i++)
        {
            //debug
            Color color = (i == index) ? Color.yellow : Color.white;

            foreach(var renderer in m_mapParents[i].GetComponentsInChildren<Renderer>())
            {
                renderer.material.color = color;
            }
        }
    }

    public void GoMapPieceSystem()
    {
        if(m_selectedIndex == -1)
        {
            Debug.Log("not select map");
            return;
        }

        SceneManager.LoadScene("MapPieceSystem");
    }
}
