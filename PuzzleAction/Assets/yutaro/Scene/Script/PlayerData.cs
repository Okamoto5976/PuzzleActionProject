using UnityEngine;

[CreateAssetMenu(fileName="PlayerData",menuName ="Game/Player Data")]
public class PlayerData : ScriptableObject
{
  [SerializeField]private  float m_normalSpeed = 5f;
  [SerializeField]private float m_dashSpeed = 10f;
  [SerializeField]private int hotbarSize = 3;

    public int HotbarSize => hotbarSize;

    public float GetSpeed(bool isDashing)
    {
        return isDashing ?m_dashSpeed :m_normalSpeed;
    }
}
