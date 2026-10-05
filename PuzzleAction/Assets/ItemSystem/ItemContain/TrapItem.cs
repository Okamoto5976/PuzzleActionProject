using UnityEngine;
[CreateAssetMenu(fileName = "SpecialItem", menuName = "Scriptable Objects/Datas/TrapItem")]
public class TrapItem : Item
{
    //pool‚©‚ç‚à‚ç‚Á‚½obj“ü‚ê‚é •Ï”
    [HideInInspector] public TrapBase TrapPrefab;

    [SerializeField] private Enum_TrapType m_enumTrap;

    public Enum_TrapType EnumTrap => m_enumTrap;

    public void SetTrap(TrapBase obj)
    {
        TrapPrefab = obj;
    }

    public override void Activation(ItemRecieveData data)
    {
        if (TrapPrefab == null)
        {
            Debug.Log("null!!");
            return;
        }

        if(m_se != null)
        {
            AudioManager.Instance.PlayAudio(m_se);

        }

        //Trap Area use item
        if (data.entity == null)
        {
            TrapPrefab.gameObject.SetActive(true);
            TrapPrefab.TrapInit();

            return;
        }
        
        TrapPrefab.gameObject.SetActive(true);
        TrapPrefab.Init(data);
        
    }
}
