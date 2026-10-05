using UnityEngine;
[CreateAssetMenu(fileName = "OtherItem", menuName = "Scriptable Objects/Datas/OtherItem")]
public class OthorItem:Item
{
    [SerializeField] private Passive m_passiveType;

    public override void AddPassive(PlayerController player)
    {
        if (m_se != null)
        {
            AudioManager.Instance.PlayAudio(m_se);

        }

        switch (m_passiveType)
        { 
            case Passive.Coupon:
                player.m_isCoupon = true;

                break;
            case Passive.MembershipCard:
                player.m_isMemberShip = true;

                break;
            default:
                Debug.LogWarning("not decide passive type");
                break;
        }
    }

    public override void RemovePassive(PlayerController player)
    {
        switch (m_passiveType)
        {
            case Passive.Coupon:
                player.m_isCoupon = false;

                break;
            case Passive.MembershipCard:
                player.m_isMemberShip = false;

                break;
            default:
                Debug.LogWarning("not decide passive type");
                break;
        }
    }
}
