using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectDataBace", menuName = "Scriptable Objects/EffectDataBace")]
public class EffectDataBace : ScriptableObject
{
    public EffectData[] m_effectDatas;

    public bool TryGet(string effectName, out EffectData data)
    {
        foreach (var effect in m_effectDatas)
        {
            if (effect.m_effectName == effectName)
            {
                data = effect;
                return true;
            }
        }

        data = default;
        return false;
    }
}
