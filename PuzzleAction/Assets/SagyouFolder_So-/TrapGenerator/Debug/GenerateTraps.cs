using System.Collections.Generic;
using UnityEngine;

public class GenerateTraps : MonoBehaviour
{
    [SerializeField] private TrapGenerator_EqualDistribution _trapGenerator;
    [SerializeField] private List<Vector3> _trapPositions;
    [SerializeField] private Middleman_Trap _trapPools;

    private void Awake()
    {
        _trapPools.InitializePool();
    }
    [ContextMenu("Generate Traps")]
    public void Generate()
    {
        Debug.Log(_trapPositions.Count);
        _trapGenerator.SpawnTraps(_trapPositions, Vector3.one, _trapPools, Enum_TrapType.Dynamite, 1);
    }
}
