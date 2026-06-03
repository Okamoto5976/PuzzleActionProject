using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HitRay
{
    public Transform m_transform;
    public float m_radius;
}

[System.Serializable]
public class HitBox
{
    public Transform m_transform;
    public float m_radius;
    [NonSerialized] public float m_distance;    // HitBoxのListを近い順に並べ替える際に使用する
    [NonSerialized] public DamageResult result; // ダメージを与える際に使用する
}

// 壁の貫通対策は未実装
public class RayCollider : MonoBehaviour
{
    [SerializeField] private int m_howManyPenetrate;    // 何体貫通するか
    [SerializeField] private float m_range;             // 射程距離(Rayをどこまで飛ばすか)

    [SerializeField] private bool m_isVisible;  // UnityEditorでHitBoxの当たり判定を表示するか

    [SerializeField] private HitRay[] hitRays;  // 攻撃判定
    [SerializeField] private HitBox[] hitBoxes; // 当たり判定

    private List<HitBox> m_hitBoxes = new();    // 当たった判定
    private HitBox m_hitBox;                    // foreachの代わり

    public void AttackCollider(DamageData data, TeamType myTeam)
    {
        // ヒットした判定のセット
        HashSet<Entity> hitSet = new();


        foreach (var hitRay in hitRays)
        {
            int i = 0;

            Entity[] hits = OverlapRay(hitRay);

            //Debug.Log($"hits.Length: {hits.Length}");

            foreach (var hit in hits)
            {
                if (hitSet.Contains(hit)) continue;
                // 同じチームなら無視
                if (hit.Team == myTeam) continue;

                hitSet.Add(hit);

                // My_ClosestPoint で使うために m_hitBox を用意
                m_hitBox = m_hitBoxes[i];
                Vector3 hitPoint = My_ClosestPoint(hit, m_hitBox, hitRay);
                Vector3 hitNormal = (hitPoint - hitRay.m_transform.position).normalized;

                m_hitBox.result = new DamageResult
                {
                    hitPoint = hitPoint,
                    hitNormal = hitNormal,

                    //overrideEffectData = m_overrideEffect,
                    //overrideAudioData = m_overrideAudio
                };

                m_hitBoxes[i].m_distance = hitPoint.x * hitPoint.x + hitPoint.y * hitPoint.y + hitPoint.z * hitPoint.z;
                i++;
            }

            // リストを昇順ソート(Rayの起点とhit位置が近い順)
            Partition(m_hitBoxes, 0, m_hitBoxes.Count - 1);

            // ヒット数が貫通制限を超えた場合、以降のHitBoxを削除する
            if (m_hitBoxes.Count > m_howManyPenetrate)
            {
                m_hitBoxes.RemoveRange(m_howManyPenetrate - 1, m_hitBoxes.Count - 1);
            }

            // ダメージを与える
            foreach (var _hitBox in m_hitBoxes)
            {
                var entity = _hitBox.m_transform.GetComponentInParent<Entity>();
                //entity.OnTakeDamage(data, _hitBox.result);
            }
        }
    }

    // HitBoxにRayが当たったかどうかを検知する関数
    // Rayは円柱の当たり判定
    // HitBoxの座標を、Rayを基準としたローカル座標で取得して、
    //  ローカル座標のX,Yで当たり判定を行う
    public Entity[] OverlapRay(HitRay hitRay)
    {
        List<Entity> hitList = new();
        foreach (var hitBox in hitBoxes)
        {
            if (hitBox.m_transform == null) continue;

            // Rayの位置と角度を基準とした相対座標(ローカル座標)
            // 例1:                                      | 例2:
            //     Ray     座標(2, 2, 2) 角度(0, 225, 0) |     Ray     座標(2, 2, 2) 角度(35.26439, 225, 0)
            //     hitBox1 座標(0, 2, 0)                 |     hitBox1 座標(0, 2, 0)
            //     hitBox2 座標(0, 0, 0)                 |     hitBox2 座標(0, 0, 0)
            // 相対座標:                                 | 相対座標:
            //     hitBox1 座標(0, 0, 2.828427)          |     hitBox1 座標(0, 0, 3.464102)
            //     hitBox2 座標(0, -2, 2.828427)         |     hitBox2 座標(0, 1.632816, 2.309526)
            Vector3 localPos = hitRay.m_transform.InverseTransformPoint(hitBox.m_transform.position);

            //Debug.Log($"localPos: {localPos}");

            // Entityの位置がRayより後ろなら無視
            if (localPos.z + hitBox.m_radius < 0) continue;
            // 射程を超えたら無視
            if (localPos.z - hitBox.m_radius > m_range) continue;

            // (相対座標の)Z座標を無視して、円形で当たり判定を行う
            if (Distance(Vector2.zero, localPos) < (hitRay.m_radius + hitBox.m_radius) * (hitRay.m_radius + hitBox.m_radius))
            {
                m_hitBoxes.Add(hitBox);
                hitList.Add(hitBox.m_transform.GetComponentInParent<Entity>());
            }
        }

        // Listを配列にする
        Entity[] hits = hitList.ToArray();

        return hits;
    }

    // HitBoxとRayの(Rayの起点から最も近い)衝突位置を、HitBoxを基準としたローカル座標で返す関数
    private Vector3 My_ClosestPoint(Entity tar, HitBox hitBox, HitRay hitRay)
    {
        Vector3 closestPoint;
        Vector3 worldClosestPoint;
        Vector3 localClosestPoint;
        Vector3 tarPos = tar.transform.position;
        Vector3 rayPos = hitRay.m_transform.position;
        float tarRad = hitBox.m_radius;
        float rayRad = hitRay.m_radius;

        // Rayを基準としたローカル座標
        Vector3 localPos = hitRay.m_transform.InverseTransformPoint(tarPos);

        // (相対座標の)Z座標が同じ地点のRayから見た位置(x, y)
        Vector2 localPos2d = localPos;
        Vector2 posNormal = localPos2d.normalized;
        Vector2 hitTarPos = posNormal * rayRad; // ターゲットが当たったRayの側面上の(Zを無視した)位置...(2)

        float edge_x = localPos.x;
        float edge_y = localPos.y;

        if (edge_x == 0) edge_x = 0.0001f;  // 0の除算回避用

        // 球上の、Rayに平行な円の角度(Z軸)
        float tarCirDir = Mathf.Atan(edge_y / edge_x) * Mathf.Rad2Deg;

        // Z座標が同じ地点のRayとRayに平行な円の距離
        float distance = Mathf.Sqrt(Distance(localPos, new Vector3(0, 0, localPos.z)));

        float z, z1, z2;
        float D = Mathf.Abs(distance - rayRad);

        // (相対座標の)X.Y座標のヒット位置
        if (localPos.x * localPos.x < hitTarPos.x * hitTarPos.x)
        {
            localClosestPoint.x = localPos.x;
        }
        else
        {
            localClosestPoint.x = hitTarPos.x;
        }
        if (localPos.y * localPos.y < hitTarPos.y * hitTarPos.y)
        {
            localClosestPoint.y = localPos.y;
        }
        else
        {
            localClosestPoint.y = hitTarPos.y;
        }

        // (相対座標の)Z座標のヒット位置
        if (distance < rayRad)
        {
            localClosestPoint.z = localPos.z - tarRad;
        }
        else
        {
            // (2)とRayに平行な円の交点
            z = Mathf.Sqrt(tarRad * tarRad - D * D);
            z1 = z + localPos.z;
            z2 = -z + localPos.z;
            //Debug.Log($"D: {D}, z1: {z1}, z2: {z2}");
            //Debug.Log($"localPos.z: {localPos.z}, z: {z}, -z: {-z}");

            // 交点の内、(相対座標の)Z座標が小さい方がヒット位置
            localClosestPoint.z = z1 < z2 ? z1 : z2;
        }

        //Debug.Log($"localClosestPoint: {localClosestPoint}");

        // 相対座標を絶対座標(ワールド座標)に変換
        worldClosestPoint = hitRay.m_transform.TransformPoint(localClosestPoint);

        // ターゲットの相対座標に変換
        closestPoint = hitBox.m_transform.InverseTransformPoint(worldClosestPoint);

        //Debug.Log($"closestPoint: {closestPoint}");

        return closestPoint;
    }

    // 2点間の距離(2D)
    private float Distance(Vector2 pos1, Vector2 pos2)
    {
        float distance = (pos2.x - pos1.x) * (pos2.x - pos1.x)
                       + (pos2.y - pos1.y) * (pos2.y - pos1.y);
        return distance;
    }

    // クイックソートで、closestPointが近い順にList内を並べ替える関数
    private void Partition(List<HitBox> hitBoxes, int left, int right)
    {
        int len = hitBoxes.Count;
        int pl = left;                            // 左カーソル
        int pr = right;                           // 右カーソル
        float x = hitBoxes[len / 2].m_distance;   // 中央の要素

        do
        {
            while (hitBoxes[pl].m_distance < x) pl++;
            while (hitBoxes[pr].m_distance > x) pr--;
            if (pl <= pr)
            {
                Swap(pl, pr);
                pl++;
                pr--;
            }
        } while (pl <= pr);

        if (left < pr) Partition(hitBoxes, left, pr);
        if (pl < right) Partition (hitBoxes, pl, right);

        //Debug.Log($"枢軸の値: {x}");
        //for (int i = 0; i < len; i++)
        //{
        //    Debug.Log($"hitBox[{i}].distance: {hitBoxes[i].m_distance}");
        //}
    }

    // 並べ替える
    private void Swap(int pl, int pr)
    {
        HitBox t = m_hitBoxes[pl];
        m_hitBoxes[pl] = m_hitBoxes[pr];
        m_hitBoxes[pr] = t;
    }

    private void OnDrawGizmos()
    {
        if (!m_isVisible) return;

        Gizmos.color = Color.blue;

        foreach (var hitBox in hitBoxes)
        {
            if (hitBox.m_transform == null) continue;
            Gizmos.DrawWireSphere(
                hitBox.m_transform.position,
                hitBox.m_radius
                );
        }
    }
}
