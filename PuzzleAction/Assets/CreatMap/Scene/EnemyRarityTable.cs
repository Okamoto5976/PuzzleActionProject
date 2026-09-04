using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRarityTable", menuName = "Scriptable Objects/GachaEngine/EnemyRarityTable")]
public class EnemyRarityTable : ScriptableObject
{
    [System.Serializable]
    public class EnemyData
    {
        public Enum_EnemyType enemyType;
        public RarityEnumAsset rarity;
    }

    [SerializeField]
    private List<EnemyData> enemies;

    public List<Enum_EnemyType> GetEnemies(RarityEnumAsset rarity)
    {
        return enemies
            .Where(x => x.rarity == rarity)
            .Select(x => x.enemyType)
            .ToList();
    }
}
