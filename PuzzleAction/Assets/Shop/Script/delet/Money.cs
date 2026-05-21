using UnityEngine;
public class Money : MonoBehaviour
{
    [SerializeField]public int m_money = 0;

    //‚¨‹à‚ð‘‚â‚·
    public void AddMoney(int amount)
    {
        m_money += amount;
        Debug.Log("‚¨‹à‚ª‘‚¦‚½" + m_money);
    }

    //‚¨‹à‚ðŽg‚¤
    public bool UseMoney(int amount)
    {
        if (m_money >= amount)
        {
            m_money -= amount;
            Debug.Log("‚¨‹à‚ðŽg‚Á‚½ : " + m_money);
            return true;
        }
        else
        {
            Debug.Log("‚¨‹à‚ª‘«‚è‚È‚¢");
            return false;
        }
    } 
}