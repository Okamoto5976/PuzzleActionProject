using UnityEngine;

public class PlayerHP : EntityHP
{
    [SerializeField] private ParticleSystem m_blood;

    [SerializeField] private EventSO m_playerDeadEvent;

    [SerializeField] private CameraManager m_cameraManager;


    private PlayerSave m_playerSave;

    protected override void Start()
    {
        base.Start();
        m_playerSave = new();

        var data = m_playerSave.LoadPlayerData();

        if(data != null )
        {
            m_currentHP = data.m_hp;
        }
    }

    public override void TakeDamage(DamageData data)
    {
        if(m_entity.CurrentState == Entity.EntityState.Dead)return;

        m_cameraManager.Shake(2f);

        base.TakeDamage(data);

        m_blood.Play();
    }

    [ContextMenu("PlayerDead")]
    protected override void Die()
    {
        Debug.Log("ゲームオーバー");

        m_playerDeadEvent.Raise();

        m_entity.ChangeState(Entity.EntityState.Dead);
        //ゲームオーバー処理実行
        //StateをDie  動かせない＋アニメーション
    }
}
