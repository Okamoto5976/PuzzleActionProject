using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrapGenerator_EqualDistribution : TrapGenerator
{
    private System.Random random = new();

    private float GetRandomValue()
    {
        return Random.value * 2 - 1;
    }

    /// <summary>
    /// subdivides cell
    /// </summary>
    /// <returns>list of center positions of subdivided cells</returns>
    private List<Vector3> SubdivideCell(Vector3 origin, Vector3 cellSize, int subdivision)
    {
        Vector3 properSquareSize = new(cellSize.x, 0, cellSize.y);
        var count = subdivision * subdivision;
        List<Vector3> result = new(new Vector3[count]);
        Vector3 adjust = -(properSquareSize / 2) + (properSquareSize / (subdivision * 2));
        Vector3 incrementSize = properSquareSize / subdivision;
        for (var i = 0; i < count; i++)
        {
            result[i] = origin + adjust;
            var xPos = incrementSize.x * (i % subdivision);
            var zPos = incrementSize.z * (i / subdivision);
            result[i] += new Vector3(xPos, 0, zPos);
            result[i] += new Vector3(incrementSize.x * GetRandomValue(), 0, incrementSize.z * GetRandomValue()) / 4;
        }
        return result;
    }

    /// <summary>
    /// spawn traps from trapPools
    /// </summary>
    /// <param name="positions">cell positions</param>
    /// <param name="cellSize">size of cell</param>
    /// <param name="trapPools">trapPools</param>
    public override void SpawnTraps(List<Vector3> positions, Vector3 cellSize, Middleman_Trap trapPools, Enum_TrapType trapType, float density)
    {
        var cellCount = positions.Count;
        List<int> itemsInCells = new(new int[cellCount]);
        var spawnCount = Mathf.Ceil(density * cellCount);
        for (var i = 0; i < spawnCount; i++)
        {
            var index = i % itemsInCells.Count;
            if (index == 0)
            {
                itemsInCells = itemsInCells.OrderBy(x => random.Next()).ToList();
            }
            itemsInCells[index]++;
        }
        for (var i = 0; i < itemsInCells.Count; i++)
        {
            var origin = positions[i];
            var itemsInCell = itemsInCells[i];
            var newPositions = SubdivideCell(origin, cellSize, itemsInCell);
            newPositions = newPositions.OrderBy(x => random.Next()).ToList();
            for (var j = 0; j < itemsInCell; j++)
            {
                var newPosition = newPositions[j];
                var obj = trapPools.GetComponent(trapType);
                obj.transform.position = newPosition;
                obj.gameObject.SetActive(true);
            }
        }
    }
}

