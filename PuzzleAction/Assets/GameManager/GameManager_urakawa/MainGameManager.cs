using System.Collections;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager Instance;

    [Header("Event")]
    [SerializeField] private BoolEventSO m_gameOverUIEvent;
    [SerializeField] private BoolEventSO m_gameClearUIEvent;

    [SerializeField] private EventSO m_playerDeadEvent;

    //[SerializeField] private EventSO m_gameOverEvent;
    //[SerializeField] private EventSO m_gameClearEvent;

    //[SerializeField] private SceneEventScript m_sceneEvent;

    [SerializeField] private StaticSceneAsset m_mapPhaseScene;

    [SerializeField] private InventorySystem m_inventorySystem;

    private PlayerSave m_playerSave;

    private bool m_isGameOver = false;
    public bool IsGameOver => m_isGameOver;

    [SerializeField] private EntityHP m_playerHP;
    [SerializeField] private PlayerController m_playerController;

    [SerializeField] private TextCanvas m_textCanvas;

    //=========tutorial===============
    private TutorialSave m_tutorialSave = new();
    [SerializeField] private TutorialManager m_tutorialManager;

    //=========gameStart==============
    [SerializeField] private GameObject m_playerRenderer;
    [SerializeField] private GameObject m_playerDirObject;

    [SerializeField] private CameraManager m_cameraManager;


    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;

        if(GameManager.Instance.IsTutorial)
        {
            GameManager.Instance.ModifyMoney(100);
        }

        //if(!GameManager.Instance.ModifyMoney(5000))
        //{
        //    Debug.LogError("ModifyMoney over ");
        //}
    }

    public IEnumerator GameStartCoroutine()
    {
        //m_playerController.gameObject.SetActive(false);
        m_playerController.SetCanMove(false);
        //m_playerController.SetIsInvincible(true);
        m_playerRenderer.SetActive(false);
        m_playerDirObject.SetActive(false);

        yield return StartCoroutine(LoadManager.m_instance.FadeIn());

        //階層表示
        yield return StartCoroutine(m_textCanvas.FadeOut());

        yield return new WaitForSecondsRealtime(1.5f);


        StartCoroutine(m_textCanvas.FadeIn());

        m_cameraManager.Shake(2f);
        m_playerController.SetCanMove(true);
        //m_playerController.gameObject.SetActive(true);
        m_playerController.SetIsInvincible(false);

        m_playerRenderer.SetActive(true);
        m_playerDirObject.SetActive(true);

        if (GameManager.Instance.IsTutorial)
        {
            Debug.Log("Tutorial");
            m_tutorialManager.PlayerControllerTutorial();
        }
        yield return null;
    }

    private void OnEnable()
    {
        m_playerDeadEvent.Register(OnPlayerDead);
    }

    private void OnDisable()
    {
        m_playerDeadEvent.Unregister(OnPlayerDead);

    }

    void Update()
    {
    
        if (m_isGameOver) return;

    }
    

    //プレイヤー死亡を受け取る
    //Event
    public void OnPlayerDead()
    {
        GameOver();
    }

    public void GameClear()
    {
        if(m_isGameOver) return;

        m_inventorySystem.Save();

        //クリア階層記録　

        PlayerData data = new PlayerData();


        //TutorialはHPを復活させる
        if (GameManager.Instance.IsTutorial)
        {
            data.m_hp = (int)m_playerController.HP;


            GameManager.Instance.SetIsTutorial(false);
            var tutorialData = m_tutorialSave.LoadTutorialData();

            if(tutorialData != null)
            {
                tutorialData = new()
                {
                    m_tutorialCompleted = false,
                    m_GoalTutorialCompleted = false,
                };
            }

            tutorialData.m_tutorialCompleted = true;
            m_tutorialSave.SaveTutorialData(tutorialData);
        }
        else
        {
            data.m_hp = m_playerHP.CurrentHP;

            GameManager.Instance.AddLevel(1);

        }
        m_playerSave = new();

        m_playerSave.SavePlayerData(data);

        LoadManager.m_instance.LoadScene(m_mapPhaseScene.Value);
    }

    //ゲームオーバー
    public void GameOver()
    {
        if (m_isGameOver) return;

        m_isGameOver = true;

        Debug.Log("ゲームオーバー");

        AudioManager.Instance.StopBGM();

        StartCoroutine(SlowTime());

        m_gameOverUIEvent.Raise(true);

    }
    private IEnumerator SlowTime()
    {
        float startTimeScale = Time.timeScale;
        float duration = 1f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;

            Time.timeScale = Mathf.Lerp(
                startTimeScale,
                0f,
                timer / duration
            );

            yield return null;
        }

        Time.timeScale = 0f;
    }
}