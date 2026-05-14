using UnityEngine;
using UnityEngine.Rendering;

public class ArrowTrap : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float m_speed = 10f;

    //飛ぶ方向
    private Vector3 m_direction;

    //飛距離
    private float m_range;

    //初期位置
    private Vector3 m_startPosition;

    
    //初期化
    public void Initialize(ArrowSpawnData data)
    {
        //出現位置
        transform.position = data.Position;

        //Noramalizeした場所
        m_direction = data.Direction.normalized;

        //飛距離
        m_range = data.Range;

        //初期位置保存
        m_startPosition=transform.position;
    }

    //Update
    private void Update()
    {
        Move();

        CheckRange();
    }


    //移動
    private void Move()
    {
        transform.position += m_direction * m_speed *Time.deltaTime;
    }

    //距離確認
    private void CheckRange()
    {
        float distance = Vector3.Distance(m_startPosition, transform.position);

        if (distance >= m_range) 
        {
            Destroy(gameObject);
        }
    }

}

