using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InfoText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_nameText;
    [SerializeField] private TextMeshProUGUI m_infoText;

    //仮　後に引数はItemData
    public void GetItemDataInfo(ItemData data)
    {
        m_nameText.text = data.ItemName;
        m_infoText.text = data.Description;

        SetPlace();
    }

    public void Reset()
    {
        m_nameText.text = null;
        m_infoText.text = null;
    }

    //仮　場所によって配置を変えてほしい
    public void SetPlace()
    {
        Vector2 pos = Mouse.current.position.ReadValue();

        transform.position = new Vector3(pos.x + 10, pos.y + 10);
    }
}
