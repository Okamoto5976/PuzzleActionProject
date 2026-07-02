using System.Collections.Generic;
using UnityEngine;

public class gaibucari : MonoBehaviour
{
    public ObjectConsolidation trapManager; 
    void Start()
    {

        List<Vector3> spawnPositions = new List<Vector3>()
    {
        new Vector3(10f, 0f, 5f),
        new Vector3(15f, 0f, 5f),
        new Vector3(5f,  0f, 5f),
        new Vector3(10f, 0f, 0f),
        new Vector3(10f, 0f, 10f)
    };
        foreach (Vector3 pos in spawnPositions)
        {
         
            trapManager.DeployTrap(pos, TrapType.Gas, new Vector2(10, 10));
        }
    }
}

