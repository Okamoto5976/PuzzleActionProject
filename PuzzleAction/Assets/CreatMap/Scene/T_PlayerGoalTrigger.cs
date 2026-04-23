using UnityEngine;
/// <summary>
/// デバッグ用スクリプト
/// </summary>
public class T_PlayerGoalTrigger : MonoBehaviour
{
    public System.Action OnReachedGoal;
    private void OnTrrigerEnter(Collider other)
    {
        //テスト
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player reached GOAL");
            OnReachedGoal?.Invoke();
        }
    }
}
