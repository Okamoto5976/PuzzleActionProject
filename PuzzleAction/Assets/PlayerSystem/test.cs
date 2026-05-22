using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private Entity Player;

    [SerializeField] private Entity attacker;
    public void OnClick()
    {
        Player.TakeDamage(attacker);
    }
}
