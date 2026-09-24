using UnityEngine;

public class DropItem : MonoBehaviour
{

    //float radius = 3f; // アイテムを拾うための半径
    [SerializeField] private float m_timeToReturn = 5f; // アイテムが自動的に戻るまでの時間
    //DropPool pool; // アイテムを管理するドロッププールのクラス
    //private GameObject prefab;
    //public event Action m_event;

    private ReturnObjectToPool m_returnObjectPool;

    private SpriteRenderer m_renderer;

    private Item m_itemData;

    public Item ItemData => m_itemData;

    private Vector3 m_velocity;

    private float m_ignoreTime = 0.5f;

    [SerializeField] private LayerMask m_wallLayer;
    [SerializeField] private LayerMask m_groundLayer;

    private bool m_isWall;
    private bool m_isGround;

    ////playerの座標が自身の半径３mいないに　プレイヤーが入ったら　プレイヤーにアイテムを渡す。
    //private void ItemGet(Collider other)
    //{
    //    if (Vector3.Distance(transform.position, other.transform.position) <= radius)
    //    {
    //        if (pool == null)
    //        {
    //            Debug.LogError("Pool is not assigned.");
    //            return;
    //        }
    //        if (other.CompareTag("Player"))
    //        {
    //            //Add.inventory();
    //            Return();
    //        }
    //    }
    //}

    private void Awake()
    {
        m_returnObjectPool = GetComponent<ReturnObjectToPool>();
        m_renderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if(m_ignoreTime > 0f)
        {
            m_ignoreTime -= Time.deltaTime;
        }

        if(!m_isGround)
        {
            transform.position += m_velocity * Time.deltaTime;

            m_velocity.y -= 9.8f * Time.deltaTime;
        }
       

    }

    //call entity get dropItem
    public void ItemGet()
    {
        Return();
    }

    public void Initialize(Item data)
    {
        m_ignoreTime = 0.5f;
        m_isWall = false;
        m_isGround = false;

        Vector3 randomDirection = new Vector3(Random.Range(-1f,1f), 0f, Random.Range(-1f,1f)).normalized;

        m_velocity = randomDirection * 3f;

        //y軸の初速
        m_velocity.y = 5f;

        //Debug.Log("Item Init");

        Invoke(nameof(Return), m_timeToReturn); // timeToReturn秒後にReturnメソッドを呼び出す
        if (data == null) return;
        SetItemData(data);
    }

    private void SetItemData(Item data)
    {
        m_itemData = data;

        m_renderer.sprite = data.icon;
    }

    private void Return()
    {
        m_returnObjectPool.ReturnToPool();
    }

    private void OnTriggerEnter(Collider other)
    {

        if ((m_wallLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            m_isWall = true;

            m_velocity.x = 0f;
            m_velocity.z = 0f;
        }

        if (m_ignoreTime > 0f) return;

        if ((m_groundLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            Debug.Log("item hit ground");
            m_isGround = true;

            m_velocity.y = 0f;
        }

    }
}
