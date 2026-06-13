using UnityEngine;

public class GridFloor : MonoBehaviour
{
    //component------------
    private Renderer m_ren;
    //--------------------

    private void Awake()
    {
        m_ren = GetComponent<Renderer>();
    }

    public void SetColor(Color color)
    {
        m_ren.material.color = color;
    }
}
