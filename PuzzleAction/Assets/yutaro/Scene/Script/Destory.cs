using UnityEngine;

public class Destory : MonoBehaviour
{
    [SerializeField] private float destroyY=-5f;

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y<destroyY)
        {
            Destroy(gameObject);
        }
    }
}
