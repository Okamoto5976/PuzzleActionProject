using UnityEngine;

public class ResultManager : MonoBehaviour
{
    // constant
    private const int MPH = 60; // minute per hour
    private const int SPM = 60; // second per minute

    [Header("Setting")]
    [SerializeField] private int m_decimalPlaces;   // for the clear time
    [SerializeField] private int m_resultDisplayInterval;

    [Header("State")]
    [SerializeField] private int m_floor;
    [SerializeField] private int m_clearTime;
    [SerializeField] private int m_killCount;
    

    
    private void Update()
    {
        
    }
}
