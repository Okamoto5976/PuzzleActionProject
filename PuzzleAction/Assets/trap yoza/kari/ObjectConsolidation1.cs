using System;
using UnityEngine;

public class ObjectConsolidation1 : MonoBehaviour
{
    public Middleman_Trap m_middleman_Trap;
    /// <summary>
    ///座標 トラップの種類 サイズを受け取り その場で親オブジェクト(箱)を作ってセットアップします
    /// 複数配置 関数をループ等で複数回呼び出し
    /// It receives the coordinates, trap type, and size, then creates and sets up the parent object (box) on the spot.
    /// To place multiple traps, call the function multiple times(e.g., within a loop).
    /// </summary>
    /// <param name="spawnPos">配置する座標 Coordinates for placement</param>
    /// <param name="requestName">プールに登録されている罠の列挙型　Name of the trap registered in the pool（Gas, Numa, Dynamite toka）</param>

    public void DeployTrap(Vector3 spawnPos, Enum_TrapType trapType, Vector2 roomSize)
    {
        if (m_middleman_Trap == null) return;

        string repuestName = trapType.ToString();
        //get pool object
        var spwanedTrap = m_middleman_Trap.GetTrap(trapType);

        if (spwanedTrap != null)
        {
            //親オブジェクト作成
            //Create parent object
            GameObject trapParentBox = new GameObject(repuestName + "_Container");
            trapParentBox.transform.position=spawnPos;

            //あっちから攫ってきたトラップを自分の子供にする
            //Make the trap brought from over there a child of the parent object that created it
            spwanedTrap.transform.SetParent(trapParentBox.transform);
            
            //座標をそろえる
            //Align the coordinates.
            spwanedTrap.transform.localPosition = Vector3.zero;

            //子供についているBoxColliderを取得する
            //Get the BoxCollider attached to the child
            BoxCollider box =spwanedTrap.GetComponent<BoxCollider>();

            //子オブジェクトから各トラップを取得
            //Retrieve each trap from the child objects
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

            // GameObject obj = spwanedTrap.GetComponent<GameObject>();
            // if (obj == null)
            // {
            //     Debug.LogError("spwanedTrap get gameobject is null");
            //     return;
            // }
            // spwanedTrap.GetComponent<GameObject>().SetActive(true);
           
            // 最後に親オブジェクトをアクティブ化
            // Finally, activate the parent object
           trapParentBox.SetActive(true);
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

