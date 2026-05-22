using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryPanel;
    [SerializeField]
    private GameObject addButton;

    // ボタンから呼ぶ
    public void ToggleInventory()
    {
        bool isOpen = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isOpen);
    }

    // 明示的に開く
    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
    }

    // 明示的に閉じる
    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
    }
}