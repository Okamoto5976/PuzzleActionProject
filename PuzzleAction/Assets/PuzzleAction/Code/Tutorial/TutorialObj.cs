using UnityEngine;

public class TutorialObj : MonoBehaviour
{
    //[System.Serializable]
    //public enum TutorialObjType
    //{
    //    Shop,
    //    Trap,
    //    Enemy
    //}

    //[SerializeField] private TutorialObjType m_type;

    //[SerializeField] private Collider m_hitCollider;

    //private bool m_isActive = true;

    //private void Start()
    //{
    //    if(m_type != TutorialObjType.Trap)
    //    {
    //        m_hitCollider.enabled = false;

    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!m_isActive) return;

    //    var target = other.GetComponent<PlayerController>();

    //    if(target == null) return;

    //    Play();
    //}

    //private void Play()
    //{
    //    m_isActive = false;

    //    switch(m_type)
    //    { 
    //        case TutorialObjType.Shop:
    //            TutorialManager.Instance.ShopAreaTutorial();
    //            break;
    //        case TutorialObjType.Trap:
    //            TutorialManager.Instance.TrapAreaTutorial();
    //            break;
    //        case TutorialObjType.Enemy:
    //            TutorialManager.Instance.EnemyAreaTutorial();
    //            break;
    //    }
    //}

    //private void Update()
    //{
    //    //if(TutorialManager.Instance.m_isEnterShop)
    //    //{
    //    //    m_hitCollider.enabled = false;
    //    //}
    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (m_type != TutorialObjType.Trap) return;

    //    var target = collision.gameObject.GetComponent<PlayerController>();

    //    if (target == null) return;

    //    TutorialManager.Instance.AttentionShopMessage(true);
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (m_type != TutorialObjType.Trap) return;

    //    var target = collision.gameObject.GetComponent<PlayerController>();

    //    if (target == null) return;

    //    TutorialManager.Instance.AttentionShopMessage(false);

    //}

}
