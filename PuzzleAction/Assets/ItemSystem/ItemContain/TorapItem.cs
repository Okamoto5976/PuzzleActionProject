using UnityEngine;
[CreateAssetMenu(fileName = "SpecialItem", menuName = "Scriptable Objects/Datas/SpecialItem")]
public class TorapItem :Item
{
    [SerializeField] private float torpvalue;

  


    public override void Activation(float value, ItemRecieveData data)
    {
        //Vector3 Corctorpos;
        
        

        Debug.Log($"‚ªã©‚Ì”ÍˆÍ‚É“ü‚Á‚Ä‹N“®‚µ‚½");
    }
}
