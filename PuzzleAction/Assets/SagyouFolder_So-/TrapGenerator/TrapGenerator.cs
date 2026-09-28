using System.Collections.Generic;
using UnityEngine;

public abstract class TrapGenerator
{
    public abstract void SpawnTraps(List<Vector3> positions, Vector3 squareSize, Middleman_Trap trapPools, Enum_TrapType trapType, float density);
}
