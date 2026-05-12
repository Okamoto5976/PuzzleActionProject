using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class T_PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private CreatMap gene;

    [SerializeField] private float speed = 4f;

    public void Initialize(CreatMap geneRef)
    {
        gene = geneRef;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        if (gene == null) return;

        // WASD / ï˚å¸ÉLÅ[
        float h = 0f;
        float v = 0f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            h = -1 / 2f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            h = 1 / 2f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            v = 1 / 2f;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            v = -1 / 2f;

        Vector3 dir = new Vector3(h, 0, v);
        if (dir.sqrMagnitude < 0.01f) return;

        Vector3 nextPos = rb.position + dir.normalized * speed * Time.fixedDeltaTime;

            rb.MovePosition(nextPos);

    }
}
