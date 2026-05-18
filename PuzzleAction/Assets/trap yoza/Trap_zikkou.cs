using UnityEngine;

public class Trap_zikkou : MonoBehaviour
{
    [Header("トラップの種類")]
    public GameObject m_TrapKinds;
    [Header("トラップの設定")]
    public float m_Range=5f;//効果半径
    public float m_Power=10f;//ガスならダメ沼地なら減速

    [ContextMenu("テスト")]
    public void DeployTrap()
    {
        if (m_TrapKinds == null)
        {
            Debug.LogWarning("トラップのプレハブがセットされていません");
            return;
        }

        //自分の位置にトラップ生成
        GameObject trapObj = Instantiate(m_TrapKinds, transform.position, Quaternion.identity);
        Debug.Log($"[TEST] オブジェクトを生成しました: {trapObj.name}");
        var gas = trapObj.GetComponent<Gas_Trap>();
        if (gas != null)
        {
            gas.Init(transform.position, m_Range, (int)m_Power);
            Debug.Log($"[SYSTEM]{gameObject.name}ががストラップ(半径:{m_Range}/ダメージ{(int)m_Power})");
            return;
        }
        var swamp =trapObj.GetComponent<Swamp_Trap>();
        if(swamp != null )
        {
            swamp.Init(transform.position, m_Range, m_Power);
            Debug.Log($"[SYSTEM]{gameObject.name}が沼地トラップ(半径:{m_Range}/減速:{(int)m_Power}倍)");
            return;
        }
    }

}
