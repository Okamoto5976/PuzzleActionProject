using UnityEngine;

public class DirectionMarker : MonoBehaviour
{
    [SerializeField]
    private PlayerController m_player;

    [SerializeField]
    private Transform m_arrow;

    [SerializeField]
    private float m_height = 0.02f;

    private void LateUpdate()
    {
        if (m_player == null)
            return;

        //Follows the player's feet
        transform.position =
            m_player.transform.position +
            Vector3.up * m_height;

        //Get input direction
        Vector3 dir = m_player.MoveDirection;

        //The arrow rotates only when there is input
        if (dir.sqrMagnitude > 0.01f)
        {
            dir.Normalize();

            m_arrow.localRotation =
                Quaternion.LookRotation(dir) *
                Quaternion.Euler(90f, 0f, 0f);
        }
    }
}