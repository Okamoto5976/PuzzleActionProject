using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float z = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(0, 0, z);
        transform.Translate(move * speed * Time.deltaTime);
    }
}