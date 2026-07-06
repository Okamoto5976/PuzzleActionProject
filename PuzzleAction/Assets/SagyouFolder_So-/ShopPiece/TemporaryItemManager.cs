using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TemporaryItemManager : MonoBehaviour
{
    [SerializeField] private EventBusAsset m_onGenerateShopInventory;

    [SerializeField] private List<Item> m_masterItemDatas;

    public List<Item> MasterItemDatas => m_masterItemDatas;

    public List<Item> GetShopItems()
    {
        return m_masterItemDatas
            .Where(x => x.Data.IsShopCompatible)
            .ToList();
    }

    public void Start()
    {
        m_onGenerateShopInventory.Trigger();
    }
}
