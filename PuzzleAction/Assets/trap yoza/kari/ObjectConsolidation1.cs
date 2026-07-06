using UnityEngine;
using System.Collections.Generic;

public class ObjectConsolidation1 : MonoBehaviour
{
    [SerializeField]
    private Middleman_Trap m_middleman_Trap;

    [Header("罠の所有者（仮でプレイヤー等のEntityをインスペクターでセットしてください）")]
    [SerializeField] private Entity m_TrapOwner;

    public void DeployTrap(List<Vector3> spawnPos, Enum_TrapType trapType, Vector2 scale)
    {
        if (m_middleman_Trap == null) return;


        foreach (Vector3 pos in spawnPos)
        {
            TrapBase trap = m_middleman_Trap.GetTrap(trapType);

            if (trap == null)
            {
                Debug.LogError($"Trap Not Found : {trapType}");
                return;
            }

            trap.transform.position = pos;

            BoxCollider box = trap.GetComponent<BoxCollider>();
            if (box != null)
            {
                if (trapType == Enum_TrapType.Gas || trapType == Enum_TrapType.Swamp)
                {
                    box.size = new Vector3(scale.x, box.size.y, scale.y);
                }
                else
                {
                    box.size = Vector3.one;
                }
            }

            trap.Init(m_TrapOwner, Vector3.forward, 1);

            trap.gameObject.SetActive(true);
        }
    }
}