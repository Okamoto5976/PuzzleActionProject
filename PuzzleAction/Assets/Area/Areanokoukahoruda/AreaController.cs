using UnityEngine;

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

    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        ExecuteEffect();
    //    }
    //}


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

           //ase AreaType.Summon:
           //   //コンポーネント探し
           //   Spawn SpawnScriput = GetComponent<Spawn>();
           //   if (SpawnScriput != null)
           //   {
           //       SpawnScriput.ActivateSpawn();
           //   }
           //   else
           //   {
           //       Debug.LogWarning("スポーンのスクリプトが見つからんアタッチしてるか？");
           //   }
           //   break;
           //
            case AreaType.Normal:
                //コンポーネント探しいる？
                Debug.Log("何もおこらない");
                break;

            //case AreaType.Shop:
            //    //コンポーネント探し
            //    shop shopScriput = GetComponent<shop>();
            //    if (shopScriput != null)
            //    {
            //        shopScriput.ActivationShop();
            //    }
            //    else
            //    {
            //        Debug.Log("ショップのスクリプトが見つからんアタッチしてるか");
            //    }
            //    break;

            case AreaType.Shop:
                shop shopScriput = GetComponent<shop>();
                if (shopScriput != null) shopScriput.ActivationShop();
                if (areaSet != null) areaSet.SetupRoom(AreaType.Shop, transform);
                break;

            case AreaType.Summon:
                Spawn SpawnScriput = GetComponent<Spawn>();
                if (SpawnScriput != null) SpawnScriput.ActivateSpawn();
                if (areaSet != null) areaSet.SetupRoom(AreaType.Summon, transform);
                break;

        }
    }
}