using UnityEngine;
//‰¼
public class Entity : MonoBehaviour
{
    public TeamType Team;
    public void OnTakeDamage(DamageData data, DamageResult result)
    {
        Debug.Log("TakeDamage");
    }
}
