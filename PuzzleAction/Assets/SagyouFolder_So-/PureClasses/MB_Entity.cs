using UnityEngine;

public class MB_Entity : MonoBehaviour
{
    [SerializeField] private MBTest_SO_Enemy enemyBlueprint;

    private NMB_EnemyInfo myEnemyInfo;
    public NMB_EnemyInfo MyEnemyInfo => myEnemyInfo;

    private void Awake()
    {
        myEnemyInfo = enemyBlueprint.TransferData();

        Debug.Log($"The original HP is {enemyBlueprint.Damage.HP}");
        Debug.Log($"My HP is {myEnemyInfo.damage.HP}");
        myEnemyInfo.damage.DealDamage(10);
        Debug.Log($"The original HP is {enemyBlueprint.Damage.HP}");
        Debug.Log($"My HP is {myEnemyInfo.damage.HP}");
    }
}

public class NMB_EnemyInfo {
    public NMB_Attack attack;
    public NMB_Damage damage;
    public NMB_Move move;
}
