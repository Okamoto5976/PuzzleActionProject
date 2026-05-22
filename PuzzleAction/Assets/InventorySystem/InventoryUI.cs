using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject m_prefabImage;
    [SerializeField] private GameObject m_Parent;

    private void Awake()
    {
        
    }

    public void InstantiateObject(int index, ItemBox data)
    {
        var obj = Instantiate(m_prefabImage, m_Parent.transform);

        var image = obj.GetComponent<ImageItem>();

        image.SetValue(index);
        image.SetData(data);

        Debug.Log("OK");
    }
}
