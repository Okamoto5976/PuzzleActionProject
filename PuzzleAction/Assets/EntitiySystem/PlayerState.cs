using UnityEngine;

public class PlayerState : EntitiyState
{
    public enum PlayerActionState
    {
        Idle,
        Attack,
        Damage,
        Dead
    }

    //Œ»İ‚ÌƒvƒŒƒCƒ„[‚Ìó‘Ô
    public PlayerActionState CurrentActionState
        = PlayerActionState.Idle;
}
