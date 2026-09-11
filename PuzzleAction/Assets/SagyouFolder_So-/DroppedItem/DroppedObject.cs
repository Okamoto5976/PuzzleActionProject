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


    private void Start()
    {
        Initialize(transform);
    }

    private void Initialize(Transform transform)
    {
        m_transform = transform;
        m_returnObjectToPool = GetComponent<ReturnObjectToPool>();
    }

    /// <summary>
    /// call to pickup this item
    /// </summary>
    /// <param name="target"></param>
    public void PickupItem(PickupItem target)
    {
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
        m_returnObjectToPool.ReturnToPool();
    }

}
