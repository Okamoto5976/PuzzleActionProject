using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class DroppedObject : MonoBehaviour
{
    [SerializeField] private float lerpSpeed = 1.0f;
    private Transform m_transform;
    private PickupItem m_target = null;
    private Vector3 m_pickupOrigin;
    private WaitForEndOfFrame m_waitForEndOfFrame = new();

    private Coroutine m_coroutine;
    private ReturnObjectToPool m_returnObjectToPool;

    public bool IsPickedUp => m_target != null;
    public bool IsReturnable => m_returnObjectToPool != null;

    //add by okamoto------------------
    #region by okamoto move moneyObj
    private float m_waitingTimer;
    public bool m_canPickUp => m_waitingTimer < 0f;
    private Rigidbody m_rd;

    [SerializeField] private LayerMask m_wallLayer;
    [SerializeField] private LayerMask m_groundLayer;

    private Vector3 m_velocity;
    private float m_ignoreTime = 0.5f;

    private bool m_isGround;
    private void Update()
    {
        if (m_ignoreTime > 0f)
        {
            m_ignoreTime -= Time.deltaTime;
        }

        if (!m_isGround)
        {
            transform.position += m_velocity * Time.deltaTime;

            m_velocity.y -= 9.8f * Time.deltaTime;
        }

        if (m_canPickUp) return;
        m_waitingTimer -= Time.deltaTime;
    }


    public void AddForce()
    {
        m_ignoreTime = 0.5f;

        m_isGround = false;

        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;

        float power = Random.Range(3f, 8f);

        m_velocity = randomDirection * power;

        //yé≤ÇÃèâë¨
        m_velocity.y = power;
    }

    private void OnTriggerEnter(Collider other)
    {

        if ((m_wallLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            //m_isWall = true;

            m_velocity.x = 0f;
            m_velocity.z = 0f;
        }

        if (m_ignoreTime > 0f) return;
        if ((m_groundLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            //Debug.Log("item hit ground");
            m_isGround = true;

            m_velocity.y = 0f;
        }

    }
    #endregion
    //------------------------------

    private void Start()
    {
        Initialize(transform);
    }
    private void Initialize(Transform transform)
    {
        m_transform = transform;
        m_returnObjectToPool = GetComponent<ReturnObjectToPool>();
        m_rd = GetComponent<Rigidbody>();
    }

    public virtual void  SetValue(int value)
    {
        m_waitingTimer = 1f;
    }


    /// <summary>
    /// call to pickup this item
    /// </summary>
    /// <param name="target"></param>
    public void PickupItem(PickupItem target)
    {
        if (!m_canPickUp) return;

        if (IsPickedUp) return;
        m_target = target;
        if (m_transform == null)
        {
            Debug.LogWarning("transform is null");
        }
        m_pickupOrigin = m_transform.position;
        if (m_coroutine != null)
        {
            StopCoroutine(m_coroutine);
        }
        StartCoroutine(GetPicked());
    }

    private IEnumerator GetPicked()
    {
        float lerp = 0;
        while (lerp < 1)
        {
            lerp += Time.deltaTime * lerpSpeed;
            var lerpValue = lerp * lerp * lerp * lerp;
            m_transform.position = Vector3.Lerp(m_pickupOrigin, m_target.transform.position, lerpValue);
            yield return m_waitForEndOfFrame;
        }
        m_target.DoPickupItem(this);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// release the object to the pool
    /// </summary>
    public void Release()
    {
        m_target = null;
        m_returnObjectToPool.ReturnToPool();
    }

}
