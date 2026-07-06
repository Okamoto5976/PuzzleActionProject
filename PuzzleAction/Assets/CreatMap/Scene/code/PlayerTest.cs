using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    [SerializeField]
    private float m_moveSpeed = 5f;

    [SerializeField]
    private float m_rotateSpeed = 150f;

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");

        transform.Rotate(
            0,
            h * m_rotateSpeed * Time.deltaTime,
            0);

        float v = Input.GetAxis("Vertical");
        transform.position +=
            transform.forward * -v * m_moveSpeed * Time.deltaTime;
    }
}