//﻿using UnityEngine;
//using System.Collections.Generic;

//public class ObjectConsolidation : MonoBehaviour
//{
//    [SerializeField]
//    private Middleman_Trap m_middleman_Trap;

//    public void DeployTrap(List<Vector3> spawnPos, Enum_TrapType trapType, Vector2 scale)
//    {
//        if (m_middleman_Trap == null) return;

//        if (m_middleman_Trap.gameObject != null)
//        {
//            m_middleman_Trap.gameObject.SendMessage("Awake", SendMessageOptions.DontRequireReceiver);

//            foreach (Transform child in m_middleman_Trap.transform)
//            {
//                child.gameObject.SendMessage("Awake", SendMessageOptions.DontRequireReceiver);
//            }
//        }

//        foreach (Vector3 pos in spawnPos)
//        {

//            Debug.Log(trapType);
//            TrapBase trap = m_middleman_Trap.GetTrap(trapType);
//            Debug.Log(trap);

//            if (trap == null)
//            {
//                Debug.LogError($"Trap Not Found : {trapType}");
//                return;
//            }

//            trap.transform.position = pos;
//            if (trap != null)
//            {
//                BoxCollider box = trap.GetComponent<BoxCollider>();
//                Gas_Trap gas = trap.GetComponent<Gas_Trap>();
//                Swamp_Trap swamp = trap.GetComponent<Swamp_Trap>();
//                Dynamite_Trap dynamite = trap.GetComponent<Dynamite_Trap>();

//                switch (trapType)
//                {
//                    case Enum_TrapType.Gas:
//                        box.size = new Vector3(scale.x, box.size.y, scale.y);
//                        gas.Init(pos, 1f, 1);
//                        break;

//                    case Enum_TrapType.Swamp:
//                        box.size = new Vector3(scale.x, box.size.y, scale.y);
//                        swamp.Init(pos, 1f, 1);
//                        break;

//                    case Enum_TrapType.Dynamite:
//                        box.size = Vector3.one;
//                        dynamite.Init(pos, 1f, 1);
//                        break;
//                }
//            }
//            trap.gameObject.SetActive(true);

//            Debug.Log($"Trap Spawn : {trapType}");
//        }
//    }

//}
//﻿//using System;
////using UnityEngine;
////
////public class ObjectConsolidation : MonoBehaviour
////{
////    public Middleman_Trap m_middleman_Trap;
////    /// <summary>
////    /// 座標と配置したい罠の列挙型を受け取って、プールからセットアップします。
////    /// 複数配置 関数をループ等で複数回呼び出し
////    /// It accepts coordinates and the name of the trap to be placed, then sets it up from the pool.
////    /// To place multiple traps, call the function multiple times(e.g., within a loop).
////    /// </summary>
////    /// <param name="spawnPos">配置する座標 Coordinates for placement</param>
////    /// <param name="requestName">プールに登録されている罠の列挙型　Name of the trap registered in the pool（Gas, Numa, Dynamite toka）</param>
////   
////    public void DeployTrap(Vector3 spawnPos, Enum_TrapType trapType,Vector2 roomSize)
////    {
////        if (m_middleman_Trap == null) return;
////
////        string repuestName = trapType.ToString();
////        //get pool object
////        TrapBase spwanedTrap = m_middleman_Trap.GetTrap(trapType);
////
////        if (spwanedTrap != null)
////        {
////            Component[] TrapPrefabCommon = spwanedTrap.GetComponents<Component>();
////            foreach (Component comp in TrapPrefabCommon)
////            {
////                if (comp is Transform || comp is BoxCollider) continue;
////
////                if (comp is Behaviour behaviour)
////                {
////                    behaviour.enabled = false;
////                }
////            }
////
////            BoxCollider box = spwanedTrap.GetComponent<BoxCollider>();
////            Gas_Trap gas = spwanedTrap.GetComponent<Gas_Trap>();
////            Swamp_Trap swamp = spwanedTrap.GetComponent<Swamp_Trap>();
////            Dynamite_Trap dynamite = spwanedTrap.GetComponent<Dynamite_Trap>();
////
////            if (trapType == Enum_TrapType.Gas || trapType == Enum_TrapType.Swamp)
////            {
////                if (box != null) box.size = new Vector3(roomSize.x, 1f, roomSize.y);
////
////                if (trapType == Enum_TrapType.Gas)
////                {
////                    if (gas != null) gas.enabled = true;
////                    gas.Init(spawnPos, 1f, 1);
////                    Debug.Log($"[AREA] {repuestName} をサイズ {roomSize} で配置");
////                }
////                else if (trapType == Enum_TrapType.Swamp)
////                {
////                    if (swamp != null) swamp.enabled = true;
////                    swamp.Init(spawnPos, 1f, 1);
////                    Debug.Log($"[AREA] {repuestName} サイズ {roomSize} で配置");
////                }
////            }
////            else
////            {
////                if (box != null) box.size = Vector3.one;
////
////                if (trapType == Enum_TrapType.Dynamite)
////                {
////                    if (dynamite != null) dynamite.enabled = true;
////                    Debug.Log($"[AREA]{repuestName}を配置");
////                }
////            }
////
////            GameObject obj = spwanedTrap.GetComponent<GameObject>();
////            if(obj == null)
////            {
////                Debug.LogError("spwanedTrap get gameobject is null");
////                return;
////            }
////            spwanedTrap.GetComponent<GameObject>().SetActive(true);
////        }
////    }
////
////    [ContextMenu("初期化")]
////    public void ClearALLTraps()
////    {
////        Gas_Trap[] allTraps = GameObject.FindObjectsByType<Gas_Trap>(FindObjectsSortMode.None);
////        foreach (var trap in allTraps)
////        {
////            Destroy(trap.gameObject);
////        }
////        Debug.Log($"[DEBUG] 画面上のすべての共通トラップ（{allTraps.Length}個）を片付けました。");
////    }
////}
////
////
