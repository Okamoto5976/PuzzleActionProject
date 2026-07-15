using UnityEngine;

public class GachaController : MonoBehaviour
{
    [SerializeField] private GachaEngine gachaEngine;

    public void DoGacha()
    {
        var result = gachaEngine.Collapse();
        Debug.Log($"You got {result.name}!");
    }
}
