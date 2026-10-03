using UnityEngine;

public class EnemyCameraDetector : MonoBehaviour
{
    public Camera mainCamera;
    [SerializeField] private LayerMask wallLayer;
    public bool EnemyVisible { get; private set; }
    public bool BossVisible { get; private set; }
    private bool previousEnemyVisible;
    private bool previousBossVisible;

    private float m_enemyTimer;
    private float m_bossTimer;



    [SerializeField] private AudioData m_enemyBGM;
    [SerializeField] private AudioData m_bossBGM;

    private AudioData m_mapBGM;

    //boss‚Ìbgm‚ð—Dæ


    [SerializeField] private float m_minX = 0.2f;
    [SerializeField] private float m_maxX = 0.8f;
    [SerializeField] private float m_minY = 0.2f;
    [SerializeField] private float m_maxY = 0.8f;

    [SerializeField] private float m_enemyAudioPlayTime;
    [SerializeField] private float m_bossAudioPlayTime;

    void Update()
    {
        if (GameManager.Instance.isShop) return;

        bool currentEnemyVisible = IsEnemyVisible();
        bool currentBossVisible = IsBossVisible();

        UpdateEnemyTimer(currentEnemyVisible);
        UpdateBossTimer(currentBossVisible);

        //previousEnemyVisible = currentEnemyVisible;
        //previousBossVisible = currentBossVisible;

        //EnemyVisible = currentEnemyVisible;
        //BossVisible = currentBossVisible;

        UpdateEnemyState();
        UpdateBossState();
    }

    private void UpdateEnemyTimer(bool value)
    {
        if(value)
        {
            m_enemyTimer += Time.deltaTime;
        }
        else
        {
            m_enemyTimer -= Time.deltaTime * 0.2f; 
        }

        m_enemyTimer = Mathf.Clamp(m_enemyTimer, 0f, m_enemyAudioPlayTime);
    }

    private void UpdateBossTimer(bool value)
    {
        if(value)
        {
            m_bossTimer += Time.deltaTime;
        }
        else
        {
            m_bossTimer -= Time.deltaTime * 0.2f;
        }

        m_bossTimer = Mathf.Clamp(m_bossTimer, 0f, m_bossAudioPlayTime);
    }

    private void UpdateEnemyState()
    {
        if (m_enemyTimer >= m_enemyAudioPlayTime && !EnemyVisible)
        {
            EnemyVisible = true;
            UpdateBGM();
        }
        else if (m_enemyTimer <= 0f && EnemyVisible)
        {
            EnemyVisible = false;
            UpdateBGM();
        }
    }

    private void UpdateBossState()
    {
        if(m_bossTimer >= m_bossAudioPlayTime && !BossVisible)
        {
            BossVisible = true;
            UpdateBGM();
        }
        else if(m_bossTimer <= 0f &&  BossVisible)
        {
            BossVisible = false;
            UpdateBGM();
        }
    }

    private void UpdateBGM()
    {
        if (BossVisible)
        {
            PlayBGM(m_bossBGM);
        }
        else if (EnemyVisible)
        {
            PlayBGM(m_enemyBGM);
        }
        else
        {
            PlayBGM(m_mapBGM);
        }
    }

    private void PlayBGM(AudioData data)
    {
        AudioManager.Instance.PlayAudio(data);
    }

    public void SetMapBGM(AudioData data)
    {
        m_mapBGM = data;
    }

    bool IsEnemyVisible()
    {
        EnemyController[] enemies =
            FindObjectsByType<EnemyController>(FindObjectsSortMode.None);

        foreach (var enemy in enemies)
        {
            Renderer renderer = enemy.GetComponentInChildren<Renderer>();

            if (renderer == null)
                continue;

            Vector3 pos = mainCamera.WorldToViewportPoint(enemy.transform.position);

            if (pos.z <= 0 ||
                pos.x < m_minX || pos.x > m_maxX ||
                pos.y < m_minY || pos.y > m_maxY)
            {
                continue;
            }


            if (Physics.Linecast(mainCamera.transform.position,
                                 enemy.transform.position,
                                 wallLayer))
            {
                continue;
            }

            return true;
        }

        return false;
    }
    bool IsBossVisible()
    {
        BossEnemyController[] bosses =
            FindObjectsByType<BossEnemyController>(FindObjectsSortMode.None);

        foreach (var boss in bosses)
        {
            Renderer renderer = boss.GetComponentInChildren<Renderer>();

            if (renderer == null)
                continue;

            Vector3 pos = mainCamera.WorldToViewportPoint(boss.transform.position);

            if (pos.z <= 0 ||
                 pos.x < m_minX || pos.x > m_maxX ||
                 pos.y < m_minY || pos.y > m_maxY)
            {
                continue;
            }


            if (Physics.Linecast(mainCamera.transform.position,
                                 boss.transform.position,
                                 wallLayer))
            {
                continue;
            }

            return true;
        }

        return false;
    }
}