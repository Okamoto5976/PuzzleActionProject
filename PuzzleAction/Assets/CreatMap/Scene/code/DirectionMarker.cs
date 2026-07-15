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

        //Follows the player's feet.
        transform.position = m_player.position + Vector3.up * m_height;

        //Align with the player's orientation.
        transform.rotation = Quaternion.Euler(
            0f,
            m_player.eulerAngles.y + 180f,
            0f);
    }
}