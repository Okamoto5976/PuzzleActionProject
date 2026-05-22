using UnityEngine;

public class RockItemTest :
    MonoBehaviour
{
    [SerializeField]
    private RockTrap m_rockPrefab;

    //キー設定
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Use();
        }
    }

    private void Use()
    {
        RockTrap rock =
            Instantiate(
                m_rockPrefab
            );

        
        RockUseData data =
            new RockUseData();

        //使用者
        data.Owner =
            gameObject;

        //出現位置
        data.Position =
            transform.position;
        //使用方向
        data.Direction =
            transform.forward;

        //範囲
        data.Range =
            10f;

        //初期化
        rock.Initialize(data);
    }
}