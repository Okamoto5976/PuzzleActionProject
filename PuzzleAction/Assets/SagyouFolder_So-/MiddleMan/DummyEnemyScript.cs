using UnityEngine;

public class DummyEnemyScript : MonoBehaviour
{

    bool isEnabled = false;
    float velocity = 0;
    float gravity = 0.001f;

    private ReturnObjectToPool rotp;

    private void Start()
    {
        rotp = GetComponent<ReturnObjectToPool>();
    }


    private void OnEnable()
    {
        isEnabled = true;
        velocity = 0;
    }

    private void OnDisable()
    {
        isEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled) return;
        velocity += gravity;

        if (transform.position.y > 0)
        {
            transform.Translate(0, -velocity, 0);
        }
        else
        {
            rotp.ReturnToPool();
        }
    }
}
