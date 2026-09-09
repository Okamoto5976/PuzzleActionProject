using UnityEngine;

public class KnockBackTrap : TrapBase
{
    [Header("KnockBack")]
    [SerializeField]
    private float m_knockBackPower = 10f;


    protected override void SetUp()
    {
        
    }


    protected override void OnHit()
    {
       
    }


    protected override void OnTriggerEnter(Collider other)
    {
        Entity target =
            other.GetComponent<Entity>();

        if (target == null)
            return;


        if (target == m_owner)
            return;


        Rigidbody targetRb =
            other.attachedRigidbody;

        if (targetRb == null)
            return;

        
        Vector3 knockBackDir =
            target.transform.position -
            transform.position;
 
        knockBackDir.y = 0f;


        targetRb.AddForce(
            knockBackDir.normalized * m_knockBackPower,
            ForceMode.Impulse);
    }
}