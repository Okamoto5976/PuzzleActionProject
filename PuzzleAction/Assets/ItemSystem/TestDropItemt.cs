using UnityEngine;

public class TestDropItem : MonoBehaviour
{
    [SerializeField] private Vector3Asset m_playerPos;

    [SerializeField] private GameObject m_dropItemObj;

    private DropItem m_dropItem;

    [SerializeField] private Item m_item;

    [ContextMenu("DropItem")]
    public void OnDrop()
    {
        GameObject obj = Instantiate(m_dropItemObj, m_playerPos.Value, Quaternion.identity);

        DropItem drop = obj.GetComponent<DropItem>();

        drop.Initialize(m_item);
    }
}
