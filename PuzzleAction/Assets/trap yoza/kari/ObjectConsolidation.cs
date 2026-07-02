using System;
using UnityEngine;

public class ObjectConsolidation : MonoBehaviour
{
    public Middleman_Trap m_middleman_Trap;
    /// <summary>
    /// 座標と配置したい罠の列挙型を受け取って、プールからセットアップします。
    /// 複数配置 関数をループ等で複数回呼び出し
    /// It accepts coordinates and the name of the trap to be placed, then sets it up from the pool.
    /// To place multiple traps, call the function multiple times(e.g., within a loop).
    /// </summary>
    /// <param name="spawnPos">配置する座標 Coordinates for placement</param>
    /// <param name="requestName">プールに登録されている罠の列挙型　Name of the trap registered in the pool（Gas, Numa, Dynamite toka）</param>
   
    public void DeployTrap(Vector3 spawnPos, Enum_TrapType trapType,Vector2 roomSize)
    {
        if (m_middleman_Trap == null) return;

        string repuestName = trapType.ToString();
        //get pool object
        var spwanedTrap = m_middleman_Trap.GetTrap(trapType);

        if (spwanedTrap != null)
        {
            Component[] TrapPrefabCommon = spwanedTrap.GetComponents<Component>();
            foreach (Component comp in TrapPrefabCommon)
            {
                if (comp is Transform || comp is BoxCollider) continue;

                if (comp is Behaviour behaviour)
                {
                    behaviour.enabled = false;
                }

            }

            BoxCollider box = spwanedTrap.GetComponent<BoxCollider>();
            Gas_Trap gas = spwanedTrap.GetComponent<Gas_Trap>();
            Swamp_Trap swamp = spwanedTrap.GetComponent<Swamp_Trap>();
            Dynamite_Trap dynamite = spwanedTrap.GetComponent<Dynamite_Trap>();

            if (trapType == Enum_TrapType.Gas || trapType == Enum_TrapType.Swamp)
            {
                if (box != null) box.size = new Vector3(roomSize.x, 1f, roomSize.y);

                if (trapType == Enum_TrapType.Gas)
                {
                    if (gas != null) gas.enabled = true;
                    gas.Init(spawnPos, 1f, 1);
                    Debug.Log($"[AREA] {repuestName} をサイズ {roomSize} で配置");
                }
                else if (trapType == Enum_TrapType.Swamp)
                {
                    if (swamp != null) swamp.enabled = true;
                    swamp.Init(spawnPos, 1f, 1);
                    Debug.Log($"[AREA] {repuestName} サイズ {roomSize} で配置");
                }
            }
            else
            {
                if (box != null) box.size = Vector3.one;

                if (trapType == Enum_TrapType.Dynamite)
                {
                    if (dynamite != null) dynamite.enabled = true;
                    Debug.Log($"[AREA]{repuestName}を配置");
                }
            }

            GameObject obj = spwanedTrap.GetComponent<GameObject>();
            if(obj == null)
            {
                Debug.LogError("spwanedTrap get gameobject is null");
                return;
            }
            spwanedTrap.GetComponent<GameObject>().SetActive(true);
        }
    }

    [ContextMenu("初期化")]
    public void ClearALLTraps()
    {
        Gas_Trap[] allTraps = GameObject.FindObjectsByType<Gas_Trap>(FindObjectsSortMode.None);
        foreach (var trap in allTraps)
        {
            Destroy(trap.gameObject);
        }
        Debug.Log($"[DEBUG] 画面上のすべての共通トラップ（{allTraps.Length}個）を片付けました。");
    }

   
}

