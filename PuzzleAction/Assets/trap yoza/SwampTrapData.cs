using UnityEngine;

[CreateAssetMenu(
    fileName = "SwampTrapData",
    menuName = "Scriptable Objects/Trap/SwampTrapData")]
public class SwampTrapData : ScriptableObject
{
    [Header("”ÍˆÍ")]
    public float m_radius = 5f;

    [Header("‚Ç‚ê‚­‚ç‚¢’x‚­‚·‚é‚©")]
    public float m_slowMultiplier = 0.5f;
}