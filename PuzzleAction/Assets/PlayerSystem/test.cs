using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private Entity Player;

    public void OnClick()
    {
        Player.TakeDamage(10);
    }
}
