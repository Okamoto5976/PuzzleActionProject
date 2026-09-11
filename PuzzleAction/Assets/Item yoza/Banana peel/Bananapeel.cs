using UnityEngine;

[RequireComponent(typeof(ReturnObjectToPool))]
public class Bananapeel : TrapBase
{
    [SerializeField] private float stunTime = 2.0f;
    //“]“|K‚à‚¿•t‚­‚¾‚ë‚¤‚©‚ç‚»‚Ì‚¯‚Â‚ÌÕŒ‚(‚¨‚Ó‚´‚¯•Ê‚É‚¢‚ç‚ñƒKƒLS‚¾)
    [SerializeField] private float ButtRange = 1.5f;
    [SerializeField] private int Buttattack = 1;
    [SerializeField] private float MomentumFalling = 2.0f;

    protected override void OnHit()
    {
        
    }

    protected override void SetUp()
    {
        

    }

    protected override void OnTriggerEnter(Collider other)
    {
        //‘Šè‚Ì”»•Ê
        if (other.GetComponentInParent<Entity>()is { } victim)
        {
            base.OnTriggerEnter(other);//ˆê‰
            //“]“|or€–S”½‰‚È‚µ
            if (victim.IsStun || victim.CurrentState == Entity.EntityState.Dead) return;
            //ƒqƒbƒgˆ—
            OnHit();
            Debug.Log("‚ ‚½‚Á‚½");
            //“]‚×
            Vector3 slipDir = (victim.transform.position - transform.position).normalized;
            victim.ApplyKnockBack(slipDir, MomentumFalling, stunTime);
            //‚¯‚ÂƒAƒ^ƒbƒN(Á‚·‚¾‚ë‚¤‚È)
            HipDrop(victim);
            //ƒv[ƒ‹•Ô‹p
            OnReturnPool();
        }
    }
    /// <summary>
    /// K‚à‚¿‚ğ‚Â‚¢‚½ÕŒ‚”g‚ÅüˆÍ‚Éƒ_ƒ[ƒW1‚ğ—^‚¦‚é
    /// </summary>T

    private void HipDrop(Entity attacker)
    {
        //‰~Œ`‚É”»’è
        Collider[] hitColliders = Physics.OverlapSphere(attacker.transform.position, ButtRange);

        foreach (var hit in hitColliders)
        {
            if (hit.TryGetComponent<Entity>(out var target))
            {
                //©•ª‚Íƒm[ƒ_ƒ
                if (target == attacker) continue;

                DamageData damageData = new DamageData
                {
                    Attack = Buttattack,
                    Attacker = attacker,
                    AttackDir = (target.transform.position - attacker.transform.position).normalized
                };
                target.TakeDamage(damageData);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ButtRange);
    }
}