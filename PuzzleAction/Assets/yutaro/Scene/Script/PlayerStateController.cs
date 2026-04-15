using NUnit.Framework.Constraints;
using UnityEngine;


public class PlayerStateController : MonoBehaviour
{
    //プレイヤーの状態
    public enum PlayerState
    {
        Normal,
        Dead
    }
    public PlayerState CurrentState { get; private set; }= PlayerState.Normal;

    public bool IsInvincible { get; private set; } = false;
    public bool CanMove { get; private set; } = true;　　　

    //状態変更
    public void SetState(PlayerState  newState)
    {
        CurrentState = newState;                            

        switch(CurrentState)
        {
            case PlayerState.Normal:
                CanMove = true;
                break;

             case PlayerState.Dead:
                CanMove = false;
                break;
        }
    }
   //無敵
   public void StartInvincible(float time)
    {
        StartCoroutine(InvincibleCoroutine(time));
    }
    private System.Collections.IEnumerator InvincibleCoroutine(float time)
    {
    IsInvincible = true;
        yield return new WaitForSeconds(time);
        IsInvincible = false;
    }
    
}
