using UnityEngine;

public class Treasure : MonoBehaviour, IInteractable
{
    [Header("Drop")]
    [SerializeField] private GachaEngine m_itemDropGachaEngine;
    private ItemManager m_itemManager;

    private bool m_isOpened = false;
    public bool IsOpened => m_isOpened;
    private void OnEnable()
    {
        m_itemManager = FindAnyObjectByType<ItemManager>();
        NULLCHECK();
    }

    private void OpenChest()
    {
        RarityEnumAsset rarity = m_itemDropGachaEngine.Collapse();
        Item item = m_itemManager.DropItem(rarity);
        if (item == null)
        {
            Debug.Log($"{this.name} : item null");
            return;
        }
        m_itemManager.DropItemSetData(transform.position, item);
        Debug.Log($"Treasure Open : {item.name}[{rarity.name}]");
    }

    public void OnInteract(Entity entity)
    {
        if (m_isOpened)
        {
            return;
        }

        m_isOpened = true;
        OpenChest();
    }

    private void NULLCHECK()
    {
        if (m_itemManager == null)
        {
            Debug.Log($"{this.name} : ItemManeger not found");
            return;
        }
        if (m_itemDropGachaEngine == null)
        {
            Debug.Log($"{this.name} : GachaEngine not found");
            return;
        }
    }
}