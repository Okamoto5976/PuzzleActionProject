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

    // ===== フラグ =====
    public bool isInvincible = false;
    public bool canMove = true;
    public bool canAttack = true;

    void Update()
    {
        DebugState(); // 確認用（あとで消してOK）
    }

    // ===== 状態変更用メソッド =====
    public void ChangeState(EnemyState newState)
    {
        currentState = newState;
    }

    // ===== ダメージ管理だけ =====
    public void SetInvincible(bool value)
    {
        isInvincible = value;
    }

    public void SetMove(bool value)
    {
        canMove = value;
    }

    public void SetAttack(bool value)
    {
        canAttack = value;
    }

    // ===== デバッグ（今の状態確認） =====
    void DebugState()
    {
        Debug.Log("State: " + currentState +
                  " | 無敵: " + isInvincible +
                  " | 移動可: " + canMove +
                  " | 攻撃可: " + canAttack);
    }
}