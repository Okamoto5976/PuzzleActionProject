using UnityEngine;
using System.Collections;

public class DashEvasion : MonoBehaviour
{
    public enum PlayerState
    {
        Normal,
        Dash
    }
    
    public float dashTime = 0.3f;

    public bool IsInvincible { get; private set; }
    public bool IsDashing => currentState == PlayerState.Dash;

    private PlayerState currentState = PlayerState.Normal;

    public void StartDash()
    {
        if (currentState == PlayerState.Normal)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        currentState = PlayerState.Dash;
        IsInvincible = true;

        yield return new WaitForSeconds(dashTime);

        IsInvincible = false;
        currentState = PlayerState.Normal;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            if (IsInvincible)
            {
                Debug.Log("ñ≥ìGèÛë‘Ç≈âÒî");
            }
            else
            {
                Debug.Log("É_ÉÅÅ[ÉW");
            }
            Destroy(other.gameObject);
        }
    }
}