using UnityEngine;

public class State : MonoBehaviour
{
    // ===== 状態 =====
    public enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Damage,
        Dead
    }

    // 現在の状態
    public EnemyState currentState = EnemyState.Idle;

    //共通のフラグ
    public bool isInvincible = false;
    public bool canMove = true;
    public bool canAttack = true;

     void Update()
     { 
            //DebugState(); // 確認用（あとで消してOK）
        
     }

    //状態変更用
    public void ChangeState(EnemyState newState)
    {
        currentState = newState;
    }

    //フラグ操作
    public void SetMove(bool value) => canMove = value;
    public void SetAttack(bool value) => canAttack = value;
    public void SetInvincible(bool value) => isInvincible = value;

    // ===== デバッグ（今の状態確認） =====
    void DebugState()
    {
        Debug.Log("State: " + currentState +
                  " | 無敵: " + isInvincible +
                  " | 移動可: " + canMove +
                  " | 攻撃可: " + canAttack);
    }
}