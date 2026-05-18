using UnityEngine;

public class ForceShift : MonoBehaviour
{
    public Vector3 offset;
    private bool toggle = false;
    void LateUpdate()
    {
        if (toggle) return;
        Vector3 o = transform.position;
        o += offset;
        transform.position = o;
        toggle = true;
    }
}
