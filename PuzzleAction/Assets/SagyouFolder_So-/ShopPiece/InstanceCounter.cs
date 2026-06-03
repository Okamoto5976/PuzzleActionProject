using UnityEngine;

[CreateAssetMenu(fileName = "InstanceCounter", menuName = "Scriptable Objects/InstanceCounter")]
public class InstanceCounter : ScriptableObject
{
    private int count;
    public int Count => count;

    public int Register()
    {
        return count++;
    }

    [ContextMenu("Reset Count")]
    public void ResetCount()
    {
        count = 0;
    }
}
