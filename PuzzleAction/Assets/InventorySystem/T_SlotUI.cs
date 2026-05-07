using UnityEngine;
using UnityEngine.UI;

public class T_SlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;

    public void SetItem(T_ItemBox item)
    {
        icon.sprite = item.data.icon;
        icon.enabled = true;
    }

    public void Clear()
    {
        icon.enabled = false;
    }
}
