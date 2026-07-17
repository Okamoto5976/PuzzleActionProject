using UnityEngine;

public class PlayerHP : EntityHP
{
    [SerializeField] private ParticleSystem m_blood;

    [SerializeField] private EventSO m_playerDeadEvent;

    public override void TakeDamage(DamageData data)
    {
        base.TakeDamage(data);

        m_blood.Play();
    }

    protected override void Die()
    {
        Debug.Log("ゲームオーバー");

        m_playerDeadEvent.Raise();

        m_entity.ChangeState(Entity.EntityState.Dead);
        //ゲームオーバー処理実行
        //StateをDie  動かせない＋アニメーション
    }
}
