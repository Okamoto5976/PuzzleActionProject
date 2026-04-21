using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MBTest_SO_Enemy", menuName = "Scriptable Objects/MBTest_SO_Enemy")]
public class MBTest_SO_Enemy : ScriptableObject
{
    [SerializeField] private NMB_Attack attack;
    [SerializeField] private NMB_Damage damage;
    [SerializeField] private NMB_Move move;

    public NMB_Attack Attack => attack;
    public NMB_Damage Damage => damage;
    public NMB_Move Move => move;

    public NMB_EnemyInfo TransferData()
    {
        NMB_EnemyInfo copy = new();
        copy.attack = attack.DeepCopy();
        copy.damage = damage.DeepCopy();
        copy.move = move.DeepCopy();

        List<int> list = new();

        var p = list.Where(x => x > 0).OrderBy(x => x).ToList();

        return copy;
    }
}
