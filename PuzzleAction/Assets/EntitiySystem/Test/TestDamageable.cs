using UnityEngine;

public class TestDamageable : MonoBehaviour
{
    [SerializeField]
    private bool m_damageble = true;

    public bool Damageable => m_damageble;
}
