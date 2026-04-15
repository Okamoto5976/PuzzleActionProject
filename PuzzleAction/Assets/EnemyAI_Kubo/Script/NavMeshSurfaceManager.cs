using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshSurfaceManager : MonoBehaviour
{
    [Header("NavMeshSurface を追加して Bake する GameObject")]
    [SerializeField]
    private List<GameObject> m_navMeshObjects = new List<GameObject>();

    private readonly List<NavMeshSurface> m_runtimeSurfaces = new List<NavMeshSurface>();

    private void Awake()
    {
        AddAndBakeNavMeshSurface();
    }

    /// <summary>
    /// 指定した GameObject に NavMeshSurface を追加し Bake する
    /// </summary>
    private void AddAndBakeNavMeshSurface()
    {
        foreach (GameObject obj in m_navMeshObjects)
        {
            if (obj == null) continue;

            // すでに NavMeshSurface があるか？
            if (obj.TryGetComponent<NavMeshSurface>(out NavMeshSurface surface)) continue;
            surface = obj.AddComponent<NavMeshSurface>();

            // 基本設定（必要に応じて調整）
            surface.collectObjects = CollectObjects.All;
            surface.useGeometry = NavMeshCollectGeometry.RenderMeshes;
            surface.overrideVoxelSize = false;
            surface.overrideTileSize = false;

            surface.BuildNavMesh();

            m_runtimeSurfaces.Add(surface);
        }
    }

    private void OnDestroy()
    {
        RemoveRuntimeNavMeshSurface();
    }

    private void RemoveRuntimeNavMeshSurface()
    {
        foreach(NavMeshSurface surface in m_runtimeSurfaces)
        {
            if(surface != null)
            {
                Destroy(surface);
            }
        }

        m_runtimeSurfaces.Clear();
    }
}
