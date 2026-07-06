using UnityEngine;

public class DirectionMarker : MonoBehaviour
{
    [SerializeField]
    private Transform m_player;

    [SerializeField]
    private Transform m_arrow;

    private void LateUpdate()
    {
        if (m_player == null)
            return;

        // プレイヤーの位置へ追従
        transform.position =
            m_player.position +
            Vector3.up * 0.02f;

        // 矢印だけ回転
        m_arrow.rotation =
            Quaternion.Euler(
                90f,
                m_player.eulerAngles.y,
                0f);
    }
}