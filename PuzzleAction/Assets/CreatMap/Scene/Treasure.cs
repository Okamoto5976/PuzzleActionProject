using UnityEngine;

public class Treasure : MonoBehaviour, IInteractable
{
    [Header("Rate Setting")]
    [SerializeField, Range(0, 1)] private float m_rareItemRate = 0.2f;

    private bool m_isOpened = false;

    public bool IsOpened => m_isOpened;

    private void Chest()
    {
        bool isRare = Random.value < m_rareItemRate;

        if (isRare)
        {
            Debug.Log("★ レアアイテムを入手！");
        }
        else
        {
            Debug.Log("□ 通常アイテムを入手！");
        }

        //return to pool OR SetActive(false)
        // Destroy(gameObject);
    }

    public void OnInteract(Entity entity)
    {
        if (m_isOpened)
        {
            return;
        }

        m_isOpened = true;
        Chest();
    }
}