using UnityEngine;

public class DirectionMarker : MonoBehaviour
{
    [SerializeField]
    private Transform m_player;

    [SerializeField]
    private float m_height = 0.02f;

    private void LateUpdate()
    {
        if (m_player == null)
            return;

        // プレイヤーの足元に追従
        transform.position = m_player.position + Vector3.up * m_height;

        // プレイヤーの向きに合わせる
        transform.rotation = Quaternion.Euler(
            0f,
            m_player.eulerAngles.y,
            0f);
    }
}