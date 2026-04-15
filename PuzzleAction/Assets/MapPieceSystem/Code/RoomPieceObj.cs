using UnityEngine;

public class RoomPieceObj : MonoBehaviour
{
    private GameObject m_parent;

    private Vector3 m_parentPos;

    private bool m_isPlace;

    public GameObject Parent { get => m_parent; }
    public Vector3 ParentPos { get => m_parentPos; }
    public bool IsPlace { get =>  m_isPlace; }

    public void SetParent(GameObject parent)
    {
        m_parent = parent;
    }

    public void SetParentPos()
    {
        m_parentPos = m_parent.transform.position;
    }

    public void SetIsPlace(bool isbool)
    {
        m_isPlace = isbool;
    }
}
