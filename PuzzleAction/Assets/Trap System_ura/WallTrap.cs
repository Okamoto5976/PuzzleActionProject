using UnityEngine;

public class WallTrap : TrapBase
{
    [Header("Wall")]
    [SerializeField] private GameObject m_wallPrefab;

    [SerializeField] private float m_wallLifeTime = 5f;


    protected override void EntitySetUp()
    {

    }

    public void Activate()
    {
        SpawnWall();
    }

    protected override void OnHit()
    {
        OnReturnPool();
    }

    private void SpawnWall()
    {
        GameObject wall =
            Instantiate(
                m_wallPrefab,
                transform.position,
                transform.rotation
            );

        Destroy(
            wall,
            m_wallLifeTime
        );
    }

   
}