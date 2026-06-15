using UnityEngine;

public class State : MonoBehaviour
{
    //状態
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
    private bool m_isInvincible = false;


    private bool m_canMove = true;
    private bool m_canAttack = true;

    public bool IsInvincible { get => m_isInvincible; }
    public bool CanMove { get => m_canMove; }
    public bool CanAttack {  get => m_canAttack; } 

    void Update()
     { 
            //DebugState(); // 確認用
        
     }

    //状態変更用
    public void ChangeState(EnemyState newState)
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