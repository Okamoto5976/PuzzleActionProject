using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Behaviour : MonoBehaviour
{
    [SerializeField] private float m_normalSpeed = 5f;
    [Header("InputSystem")]
    [SerializeField] private InputActionReference m_action;
    [SerializeField] private InputActionReference m_dashAction;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.3f;
    [SerializeField] private DashEvasion dashEvasion;
    [SerializeField] private EntityMove m_move;

    private bool isDashing;
    private Vector3 dashVelocity;
    private Vector3 lastMoneDir = Vector3.forward;
    private Vector3 m_movement;


    private void Update()
    {
        //移動処理
        Vector2 input = m_action.action.ReadValue<Vector2>();
        m_movement = new Vector3(input.x, 0f, input.y);

        if (m_dashAction.action.triggered && !isDashing)
        {
            dashEvasion.StartDash();
            StartCoroutine(Dash());
        }
        if(m_movement.sqrMagnitude>0.01f)
        {
            lastMoneDir=m_movement.normalized;
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;

        float timer = 0f;

        while (timer < dashTime)
        {
            //ダッシュ分の速度を作る
            Vector3 dashDir ;
            if(m_movement.sqrMagnitude>0.01f)
            {
                dashDir = m_movement.normalized;
            }
            else
            {
                dashDir = lastMoneDir;
            }

                dashVelocity = dashDir * dashSpeed;

            timer += Time.deltaTime;
            yield return null;
        }
        dashVelocity = Vector3.zero;
        isDashing = false;
    }

    public void Move(Vector3 dir, float speed)
    {
        m_move.OnMove(dir, speed);
    }

    private void FixedUpdate()
    {
        //通常移動
        Vector3 normalMove = m_movement * m_normalSpeed;

        //合成（通常＋ダッシュ）
        Vector3 finalMove = normalMove + dashVelocity;

        //方向と速さに分解して渡す
        if (finalMove != Vector3.zero)
        {
            m_move.OnMove(finalMove.normalized,finalMove.magnitude);
        }
    }
}
