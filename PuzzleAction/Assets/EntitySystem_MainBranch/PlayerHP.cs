using UnityEngine;

public class PlayerHP : EntityHP
{
    protected override void Die()
    {
        Debug.Log("Player has died");
    }
}
