using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Dynamite_Trap : MonoBehaviour
{
    private Vector3 m_Center;
    private float m_Radius;
    private int m_DamageAmount;

    [Header("起動から爆発までの時間")]
    [SerializeField] private float m_ExplosionDelay = 3.0f;
    private float m_Timer = 0f;
    private bool m_IsTriggered = false;//起爆スイッチが入ったか

    ///<summary>
    ///アイテム側から呼ぶ初期化
    /// </summary>
    public void Init(Vector3 pos, float radius, float power)
    {
        m_Center = pos;
        m_Radius = radius;
        m_DamageAmount = (int)power;

        transform.position = pos;
    }
    void Update()
    {
        if (m_IsTriggered)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer >= m_ExplosionDelay)
            {
                Explosion();//ドカン！！！
            }
        }
    }
    /// <summary>
    /// テスト用　触れたら起爆にしたほうが攻撃したら起爆に近いと思ったから どっちも触れてはいる(消す)
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        //すでに起爆なら何もしない
        if (m_IsTriggered) return;
        karitesuto target=other.GetComponent<karitesuto>();
        if (target != null)
        {
            if(target.m_MyTeam!=TrapTeam.Nature)
            {
                Debug.Log($"[DYNAMITE] ➔ {target.gameObject.name}({target.m_MyTeam})がダイナマイトに触れた！");
                TriggerExplosion();
            }
        }
    }
    /// <summary>
    /// 起爆スイッチ　ポチー
    /// </summary>
    public void TriggerExplosion()
    {
        if (m_IsTriggered) return;

        m_IsTriggered = true;
        Debug.Log($"[DYMAITTE]{gameObject.name}がダメージをウケた！{m_ExplosionDelay}秒後に爆発");
    }
    /// <summary>
    /// 爆発処理
    /// </summary>
    private void Explosion()
    {
        Debug.Log("[DYNAMITE]💥ドォーーーン！！！爆破！");

        //シーン内のすべてのターゲット検索
        karitesuto[] allTargets = Object.FindObjectsByType<karitesuto>(FindObjectsSortMode.None);
        foreach (var traget in allTargets)
        {
            if (traget == null) continue;
            float distance = Vector3.Distance(m_Center, traget.transform.position);

            if (distance <= m_Radius)
            {
                if (traget.m_MyTeam != TrapTeam.Nature)
                {
                    Debug.Log($"[DYNAMITE]→{traget.gameObject.name}({traget.m_MyTeam})に{m_DamageAmount}の爆発ダメージ!");
                }
            }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = m_IsTriggered?Color.yellow:Color.red;

        float visualRadius = m_Radius>0f?m_Radius:5f;
        Gizmos.DrawWireSphere(transform.position,visualRadius);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = m_IsTriggered ? Color.yellow : Color.red;
    //    float visualRadius = m_Radius > 0f ? m_Radius : 5f;
    //    Gizmos.DrawWireSphere(transform.position, visualRadius);
    //}
}   //