using TMPro;
using UnityEngine;

public class StatusView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_text;

    [SerializeField] private Entity m_player;
    [SerializeField] private EntityHP m_playerHP;

    [SerializeField] private EntityData m_playerData;

    private string m_hp;
    private string m_maxHP;
    private string m_str;
    private string m_def;
    private string m_speed;
    private string m_cr;
    private string m_cd;
    private string m_br;
    private string m_kb;
    private string m_sd;
    private string m_sres;
    private string m_pres;
    private string m_gres;
    private string m_bres;
    private string m_stunres;

    private void Update()
    {
        m_hp = m_playerHP.CurrentHP.ToString();
        m_maxHP = FormatDifference(m_player.HP - m_playerData.HP);
        m_str = FormatDifference(m_player.STR - m_playerData.STR);
        m_def = FormatDifference(m_player.DEF - m_playerData.DEF);
        m_speed = FormatDifference(m_player.Speed -  m_playerData.Speed);
        m_cr = FormatDifference(m_player.CriticalRate - m_playerData.CriticalRate);
        m_cd = FormatDifference(m_player.CriticalDamage - m_playerData.CriticalDamage);
        m_br = FormatDifference(m_player.BreakRate - m_playerData.BreakRate);
        m_kb = FormatDifference(m_player.KnockBack -  m_playerData.KnockBack);
        m_sd = FormatDifference(m_player.StunPower - m_playerData.StunDuration);
        m_sres = FormatDifference(m_player.SwampRes - m_playerData.SwampRes);
        m_pres = FormatDifference(m_player.PoisonRes - m_playerData.PoisonRes);
        m_gres = FormatDifference(m_player.GasRes - m_playerData.GasRes);
        m_bres = FormatDifference(m_player.BurnRes - m_playerData.BurnRes);
        m_stunres = FormatDifference(m_player.StunRes - m_playerData.StunRes);

        m_text.text =
            "<mspace=20>" +
            $"HP              {m_hp}\n" +
            $"MAXHP           {m_playerData.HP} {m_maxHP}\n" +
            $"STR             {m_playerData.STR} {m_str}\n" +
            $"DEF             {m_playerData.DEF} {m_def}\n" +
            $"SPEED           {m_playerData.Speed} {m_speed}\n" +
            $"CRITICALRATE    {m_playerData.CriticalRate} {m_cr}\n" +
            $"CRITICALDAMAGE  {m_playerData.CriticalDamage} {m_cd}\n" +
            $"BREAKRATE       {m_playerData.BreakRate} {m_br}\n" +
            $"KNOCKBACK       {m_playerData.KnockBack} {m_kb}\n" +
            $"STUNDURATION    {m_playerData.StunDuration} {m_sd}\n" +
            $"SWAMPRES        {m_playerData.SwampRes} {m_sres}\n" +
            $"POISONRES       {m_playerData.PoisonRes} {m_pres}\n" +
            $"GASRES          {m_playerData.GasRes} {m_gres}\n" +
            $"BURNRES         {m_playerData.BurnRes} {m_bres}\n" +
            $"STUNRES         {m_playerData.StunRes} {m_stunres}\n";
    }

    private string FormatDifference(float difference)
    {
        if (difference > 0)
            return $"<color=green>+{difference}</color>";

        if (difference < 0)
            return $"<color=red>{difference}</color>";

        return "";
    }
}
