using UnityEngine;
using UnityEngine.InputSystem.Users;

public enum AreaType
{
    Damage,
    Summon,
    Normal,
    Shop
}

public class AreaController : MonoBehaviour
{
    public AreaType m_CurrentArea;
    private bool m_Generate = false;
     void Start()
    {
        Setup();
    }
    private void Setup()
    {
        AreaSet areaSet =GetComponent<AreaSet>();
        if (areaSet != null)
        {
            areaSet.SetupRoom(m_CurrentArea, transform);
        }
    }

    public void ExecuteEffect()
    {
         AreaSet areaSet = GetComponent<AreaSet>();

        switch (m_CurrentArea)
        {
            case AreaType.Damage:
                //コンポーネント探し
                damage damageScript = GetComponent<damage>();
                if (damageScript != null)
                {
                    damageScript.ActivateDamage();
                }
                else
                {
                    Debug.LogWarning("ダメージのスクリプトが見つからんアタッチしてるか？");
                }
                break;

            case AreaType.Summon:
                //コンポーネント探し
                Spawn SpawnScriput = GetComponent<Spawn>();
                if (SpawnScriput != null)
                {
                    SpawnScriput.ActivateSpawn();
                }
                else
                {
                    Debug.LogWarning("スポーンのスクリプトが見つからんアタッチしてるか？");
                }
                break;

            case AreaType.Normal:
                //コンポーネント探しいる？
                Debug.Log("何もおこらない");
                break;

            case AreaType.Shop:
                //コンポーネント探し
                shop shopScriput = GetComponent<shop>();
                if (shopScriput != null)
                {
                    shopScriput.ActivationShop();
                }
                else
                {
                    Debug.Log("ショップのスクリプトが見つからんアタッチしてるか");
                }
                break;
               
                
        }
    }
}