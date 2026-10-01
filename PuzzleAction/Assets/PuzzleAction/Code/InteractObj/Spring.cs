using UnityEngine;
using System.Collections.Generic;

public class Spring : MonoBehaviour, IInteractable
{
    private bool m_isActive = false;

    [SerializeField] private List<BuffSetting> m_buffList = new();

    public void OnInteract(Entity entity)
    {
        if (m_isActive) return;

        //effect
        int num = Random.Range(0, m_buffList.Count);

        var buff = m_buffList[num];


        StatusModifier modifier = new StatusModifier()
        {
            m_statType = buff.m_statusType,
            m_value = buff.m_value,
            m_modType = buff.m_modifierType,
        };

        entity.AddBuff(modifier, buff.m_buffID, buff.m_duration);

        m_isActive = true;
    }
}
