using UnityEngine;
using System.Collections.Generic;

public class ObjectConsolidation1 : MonoBehaviour
{
    [SerializeField]
    private Middleman_Trap m_middleman_Trap;

    public void DeployTrap(List<Vector3> spawnPos, Enum_TrapType trapType, Vector2 scale)
    {
        if (m_middleman_Trap == null) return;

        if (m_middleman_Trap.gameObject != null)
        {
            m_middleman_Trap.gameObject.SendMessage("Awake", SendMessageOptions.DontRequireReceiver);

            foreach (Transform child in m_middleman_Trap.transform)
            {
                child.gameObject.SendMessage("Awake", SendMessageOptions.DontRequireReceiver);
            }
        }

        foreach (Vector3 pos in spawnPos)
        {

            Debug.Log(trapType);
            TrapBase trap = m_middleman_Trap.GetTrap(trapType);
            Debug.Log(trap);

            if (trap == null)
            {
                Debug.LogError($"Trap Not Found : {trapType}");
                return;
            }

            trap.transform.position = pos;
            if(trap != null)
            {
                BoxCollider box = trap.GetComponent<BoxCollider>();
                Gas_Trap gas = trap.GetComponent<Gas_Trap>();
                Swamp_Trap swamp = trap.GetComponent<Swamp_Trap>();
                Dynamite_Trap dynamite = trap.GetComponent<Dynamite_Trap>();

                switch(trapType)
                {
                    case Enum_TrapType.Gas:
                        box.size = new Vector3(scale.x, box.size.y, scale.y);
                        gas.Init(pos, 1f, 1);
                        break;

                    case Enum_TrapType.Swamp:
                        box.size = new Vector3(scale.x, box.size.y, scale.y);
                        swamp.Init(pos, 1f, 1);
                        break;

                    case Enum_TrapType.Dynamite:
                        box.size = Vector3.one;
                        dynamite.Init(pos, 1f, 1);
                        break;
                }
            }
            trap.gameObject.SetActive(true);

            Debug.Log($"Trap Spawn : {trapType}");
        }
    }

}