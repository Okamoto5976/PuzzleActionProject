using UnityEngine;

public class ArrowItemTest : MonoBehaviour
{
    [SerializeField]
    private ArrowTrap m_arrowPrefab;

    [SerializeField]
    private Entity m_owner;

    //キー設定
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Use();
        }

    }

    private void Use()
    {
        ArrowTrap arrow = Instantiate(
            m_arrowPrefab
            );

        ArrowUseData data = new();

        //使用者
        data.Owner =
            gameObject;

        //出現位置
        data.Position =
            transform.position;

        //使用方向
        data.Direction =
        m_owner.transform.forward;

        //範囲
        data.Range = 10f;

        //初期化
        arrow.Initialize( data );
    }
}
