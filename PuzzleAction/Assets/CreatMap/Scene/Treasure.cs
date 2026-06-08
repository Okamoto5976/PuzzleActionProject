using UnityEngine;

public class Treasure : MonoBehaviour
{
    [Header("Rate Setting")]
    [SerializeField, Range(0, 1)] private float m_rareItemRate = 0.2f;

    //SO
    private PlayerController m_player;

    private bool m_isOpened = false;
    private bool m_isMimic = false;

    private void Start()
    {
        m_player = FindAnyObjectByType<PlayerController>();
    }

    public void SetIsMimic(bool isMimic)
    {
        m_isMimic = isMimic;
    }

    private void Update()
    {
        if (m_isOpened) return;

        //距離チェック
        float distance = Vector3.Distance(transform.position, m_player.transform.position);
        if (distance > 2f) return;

        //Debug
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_isOpened = true;

            if (m_isMimic)
            {
                Mimic();
            }
            else
            {
                Chest();
            }
        }

    }

    private void Mimic()
    {
        Debug.Log(" 〇 ミミックだった！敵出現！");

        //delete Treasure box 
        //Generate mimic <- Pool Manager
        Destroy(gameObject);
    }

    private void Chest()
    {
        bool isRare = Random.value < m_rareItemRate;

        if (isRare)
        {
            Debug.Log(" ★ レアアイテムを入手！");
        }
        else
        {
            Debug.Log(" □ 通常アイテムを入手！");
        }

        // 宝箱消える
        //Destroy(gameObject);
    }
}
