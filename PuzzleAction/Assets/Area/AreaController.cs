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

    public AreaType CurrentArea;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ExecuteEffect();
        }
    }

    void ExecuteEffect()
    {
        switch (CurrentArea)
        {
            case AreaType.Damage:
                //Debug.Log("継続ダメ受けた痛");
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
                // Debug.Log("召喚");
                //コンポーネント探し
                Spawn SpawnScriput = GetComponent<Spawn>();
                if (SpawnScriput != null)
                {
                    SpawnScriput.ActivateSpawn();
                }
                break;

            case AreaType.Normal:
                //コンポーネント探しいる？
                Debug.Log("なんもない面白味もないw");
                break;

            case AreaType.Shop:
                Debug.Log("買い物しよ...盗みはできないのかな");
                //コンポーネント探し
                shop shopScriput = GetComponent<shop>();
                if (shopScriput != null)
                {
                    //shopScriput.
                }
                break;
        }
    }
}
