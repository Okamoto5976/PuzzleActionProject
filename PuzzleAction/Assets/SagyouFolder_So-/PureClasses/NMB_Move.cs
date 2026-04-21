using UnityEngine;

[System.Serializable]
public class NMB_Move : NMB_MBAdapter<NMB_Move>
{
    [Header("Move")]
    [SerializeField] private int m_speed;
    [SerializeField] private Vector3 m_direction;

    public void Awake()
    {

    }

    public void Update()
    {

    }
}
