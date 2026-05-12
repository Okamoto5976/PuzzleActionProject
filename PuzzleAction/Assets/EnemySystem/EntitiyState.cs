using UnityEngine;

abstract public class EntitiyState : MonoBehaviour
{
    //状態
    public enum EntityState
    {
        Idle,
        Attack,
        Damage,
        Dead
    }

    // 現在の状態
    public EntityState currentState = EntityState.Idle;

    //共通のフラグ
    protected bool m_isInvincible = false;
    protected bool m_canMove = true;
    protected bool m_canAttack = true;

    //外部取得
    public bool IsInvincible { get => m_isInvincible; }
    public bool CanMove { get => m_canMove; }
    public bool CanAttack { get => m_canAttack; }

    void Update()
    {
        //DebugState(); // 確認用

    }

    //状態変更用
    public void ChangeState(EntityState newState)
    {
        currentState = newState;
    }

    //フラグ操作
    public void SetCanMove(bool value) => m_canMove = value;
    public void SetCanAttack(bool value) => m_canAttack = value;
    public void SetIsInvincible(bool value) => m_isInvincible = value;

    //デバッグ
    void DebugState()
    {
        Debug.Log("State: " + currentState +
                  " | 無敵: " + m_isInvincible +
                  " | 移動可: " + m_canMove +
                  " | 攻撃可: " + m_canAttack);
    }
}
