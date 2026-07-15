using UnityEngine;

public class PlayerHP : EntityHP
{
    [SerializeField] private ParticleSystem m_blood;

    public override void TakeDamage(DamageData data)
    {
        base.TakeDamage(data);

        m_blood.Play();
    }

    protected override void Die()
    {
        Debug.Log("ゲームオーバー");
        //ゲームオーバー処理実行
        //StateをDie  動かせない＋アニメーション
    }
}
