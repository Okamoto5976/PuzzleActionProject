using UnityEngine;
using System.Collections.Generic;

public class Spring : MonoBehaviour, IInteractable
{
    private bool m_isActive = false;

    [SerializeField] private List<BuffSetting> m_buffList = new();

    [SerializeField] private GachaEngine m_itemDropGachaEngine;

    [SerializeField] private DropMoneyEventSO m_dropMoneyEventSO;
    [SerializeField] private int m_minMoney;
    [SerializeField] private int m_maxMoney;

    [SerializeField] private ParticleSystem m_particle;
    [SerializeField] private AudioData m_se;


    public void OnInteract(Entity entity)
    {
        if (m_isActive) return;

        int num = Random.Range(0, 3);

        switch (num)
        { 
            case 0:
                DropMoney();
                break;

            case 1:
                DropItem();
                break;

            case 2:
                GiftBuff(entity);
                break;
        }

        m_particle.Play();
        AudioManager.Instance.PlayAudio(m_se);
        

        m_isActive = true;
    }

    private void DropMoney()
    {
        int money = Random.Range(m_minMoney, m_maxMoney);

        m_dropMoneyEventSO.Raise(transform.position, money);

    }

    private void DropItem()
    {
        RarityEnumAsset rarity = m_itemDropGachaEngine.Collapse();
        Item item = ItemManager.Instance.DropItem(rarity);
        if (item == null)
        {
            Debug.Log($"{this.name} : item null");
            return;
        }
        ItemManager.Instance.DropItemSetData(transform.position, item);
    }

    private void GiftBuff(Entity entity)
    {
        //effect
        int index = Random.Range(0, m_buffList.Count);

        var buff = m_buffList[index];


        StatusModifier modifier = new StatusModifier()
        {
            m_statType = buff.m_statusType,
            m_value = buff.m_value,
            m_modType = buff.m_modifierType,
        };

        entity.AddBuff(modifier, buff.m_buffID, buff.m_duration);
    }
}
