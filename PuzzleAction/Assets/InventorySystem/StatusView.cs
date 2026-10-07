using TMPro;
using UnityEngine;

public class StatusView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_text;

    [SerializeField] private Entity m_player;
    [SerializeField] private EntityHP m_playerHP;

    [SerializeField] private EntityData m_playerData;

    private string m_Hp;

    private string m_maxHP => FormatBaseValueDifference(m_player.HP, m_playerData.HP);
    private string m_STR => FormatBaseValueDifference(m_player.STR, m_playerData.STR);
    private string m_DEF => FormatBaseValueDifference(m_player.DEF, m_playerData.DEF);
    private string m_SPEED => FormatBaseValueDifference(m_player.Speed, m_playerData.Speed);
    private string m_CR => FormatBaseValueDifference(m_player.CriticalRate, m_playerData.CriticalRate);
    private string m_CD => FormatBaseValueDifference(m_player.CriticalDamage, m_playerData.CriticalDamage, "F1");
    private string m_BR => FormatBaseValueDifference(m_player.BreakRate, m_playerData.BreakRate);
    private string m_SWAMPRES => FormatBaseValueDifference(m_player.SwampRes, m_playerData.SwampRes);
    private string m_POISONRES => FormatBaseValueDifference(m_player.PoisonRes, m_playerData.PoisonRes);
    private string m_GASRES => FormatBaseValueDifference(m_player.GasRes, m_playerData.GasRes);
    private string m_BURNRES => FormatBaseValueDifference(m_player.BurnRes, m_playerData.BurnRes);
    private string m_STUNRES => FormatBaseValueDifference(m_player.StunRes, m_playerData.StunRes);

    //difference
    private string m_difMaxHP;
    private string m_difStr;
    private string m_difDef;
    private string m_difSpeed;
    private string m_difCr;
    private string m_difCd;
    private string m_difBr;
    private string m_difSres;
    private string m_difPres;
    private string m_difGres;
    private string m_difBres;
    private string m_difStunres;

    private void Update()
    {
        m_Hp = m_playerHP.CurrentHP.ToString();
        m_difMaxHP = FormatDifference(m_player.HP - m_playerData.HP);
        m_difStr = FormatDifference(m_player.STR - m_playerData.STR);
        m_difDef = FormatDifference(m_player.DEF - m_playerData.DEF);
        m_difSpeed = FormatDifference(m_player.Speed -  m_playerData.Speed);
        m_difCr = FormatDifference(m_player.CriticalRate - m_playerData.CriticalRate);
        m_difCd = FormatDifference(m_player.CriticalDamage - m_playerData.CriticalDamage,"F1");
        m_difBr = FormatDifference(m_player.BreakRate - m_playerData.BreakRate);
        m_difSres = FormatDifference(m_player.SwampRes - m_playerData.SwampRes);
        m_difPres = FormatDifference(m_player.PoisonRes - m_playerData.PoisonRes);
        m_difGres = FormatDifference(m_player.GasRes - m_playerData.GasRes);
        m_difBres = FormatDifference(m_player.BurnRes - m_playerData.BurnRes);
        m_difStunres = FormatDifference(m_player.StunRes - m_playerData.StunRes);

        m_text.text =
            "<mspace=15>" +
            $"HP              {m_Hp,6}\n" +
            $"MAXHP           {m_maxHP,6} {m_difMaxHP,8}\n" +
            $"STR             {m_STR,6} {m_difStr,8}\n" +
            $"DEF             {m_DEF,6} {m_difDef,8}\n" +
            $"SPEED           {m_SPEED,6} {m_difSpeed,8}\n" +
            $"CRITICALRATE    {m_CR,6} {m_difCr,8}\n" +
            $"CRITICALDAMAGE  {m_CD,6} {m_difCd,8}\n" +
            $"BREAKRATE       {m_BR,6} {m_difBr,8}\n" +
            $"SWAMPRES        {m_SWAMPRES,6} {m_difSres,8}\n" +
            $"POISONRES       {m_POISONRES,6} {m_difPres,8}\n" +
            $"GASRES          {m_GASRES,6} {m_difGres,8}\n" +
            $"BURNRES         {m_BURNRES,6} {m_difBres,8}\n" +
            $"STUNRES         {m_STUNRES,6} {m_difStunres,8}\n";
    }

    private string FormatDifference(float difference, string format = "F0")
    {
        string text = difference > 0
            ? $"+{difference.ToString(format)}"
            : difference.ToString(format);

        text = $"({text})".PadLeft(8);

        if (difference > 0)
            return $"<color=green>{text}</color>";

        if (difference < 0)
            return $"<color=red>{text}</color>";

        return text;
    }
    private string FormatBaseValueDifference(float value, float baseValue, string format = "F0")
    {
        string text = value.ToString(format).PadLeft(6);

        if (value > baseValue)
            return $"<color=green>{text}</color>";

        if (value < baseValue)
            return $"<color=red>{text}</color>";

        return text;
    }
}
