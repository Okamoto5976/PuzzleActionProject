using UnityEngine;

[CreateAssetMenu(fileName="PlayerData",menuName ="Game/Player Data")]
public class PlayerData : ScriptableObject
{
  public  float m_normalSpeed = 5f;
  public float m_dashSpeed = 10f;
  public int hotbarSize = 3;

    public float GetSpeed(bool isDashing)
    {
        return isDashing ?m_dashSpeed :m_normalSpeed;
    }
}
