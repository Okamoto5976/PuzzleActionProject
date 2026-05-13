using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class __PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ);
        Vector3 next = transform.position + move * speed * Time.fixedDeltaTime;
        if (NavMesh.SamplePosition(next, out NavMeshHit hit, 1, NavMesh.AllAreas))
            rb.MovePosition(next);
    }
}