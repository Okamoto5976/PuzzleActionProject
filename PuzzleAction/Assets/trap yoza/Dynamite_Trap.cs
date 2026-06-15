using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Dynamite_Trap : Entity
{
    private Vector3 m_Center;
    private float m_Radius;
    private int m_DamageAmount;

    [Header("起動から爆発までの時間")]
    [SerializeField] private float m_ExplosionDelay = 3.0f;
    private float m_Timer = 0f;
    private bool m_IsTriggered = false;//起爆スイッチが入ったか

    private Entity m_dinamite;

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
    protected override void Update()
    {
        base.Update();

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
        Entity target = other.GetComponent<Entity>();
        if (target == null)
        {
            return;
        }
        if (target.Team == TeamType.Nature)
        {
            return;
            //Debug.Log($"[DYNAMITE] ➔ {target.gameObject.name}({target.m_MyTeam})がダイナマイトに触れた！");
        }
        TriggerExplosion();
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

        // シーン内のすべてのターゲット検索
        Entity[] allTargets = Object.FindObjectsByType<Entity>(FindObjectsSortMode.None);
        foreach (var target in allTargets)
        {
            if (target == null || target == this) continue; // 自分自身（ダイナマイト）は除外

            float distance = Vector3.Distance(m_Center, target.transform.position);

            if (distance <= m_Radius)
            {
                // 自分と同じチームにはダメージを与えない（フレンドリーファイアなしの場合）
                if (target.Team == this.Team) continue;

                // 敵チームにダメージを与える
                //target.TakeDamage(m_DamageAmount);
                Debug.Log($"[DYNAMITE] ➔ {target.gameObject.name} に {m_DamageAmount} の爆発ダメージ！");
            }
        }

        // 自分自身を消去
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = m_IsTriggered ? Color.yellow : Color.red;

        float visualRadius = m_Radius > 0f ? m_Radius : 5f;
        Gizmos.DrawWireSphere(transform.position, visualRadius);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = m_IsTriggered ? Color.yellow : Color.red;
    //    float visualRadius = m_Radius > 0f ? m_Radius : 5f;
    //    Gizmos.DrawWireSphere(transform.position, visualRadius);
    //}
}   //