using UnityEngine;

public class Treasure : MonoBehaviour
{
    [Header("Rate Setting")]
    [SerializeField, Range(0, 1)] private float m_rareItemRate = 0.2f;

    [SerializeField] private Vector3Asset m_player;

    private bool m_isOpened = false;

    private void Update()
    {
        if (m_isOpened) return;

        float distance = Vector3.Distance(transform.position,m_player.Value);

        if (distance > 2f) return;

        //Debug
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_isOpened = true;

            Chest();
        }
    }

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
}