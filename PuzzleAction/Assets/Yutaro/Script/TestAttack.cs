using UnityEngine;

public class TestAttack : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        EntityHP.IDamage damageable = collision.gameObject.GetComponent<EntityHP.IDamage>();

        if (damageable != null)
        {
            EntityHP.DamageData data = new EntityHP.DamageData
            {
                damage = 10,
                type = EntityHP.DamageType.Normal,
                knockbackForce = 5f,
                hitPoint = collision.contacts[0].point
            };

            damageable.TakeDamage(data);
        }
    }
}