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

    //=========tutorial===============
    private TutorialSave m_tutorialSave = new();


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
    

        //if (Keyboard.current.mKey.wasPressedThisFrame)
        //{
        //    SceneManager.LoadScene("MapSelectionPhase");

        //}
        //ゲームオーバー後に止める
        if (m_isGameOver) return;

        //timemanager.DecreaseValue(Time.deltaTime);

        //デバック用
        //Debug.Log($"Score: {m_scoreRuntime.Value} | Money: {m_moneyRuntime.Value} | Time: {timemanager.Value:F1}");
        
        //時間切れ
        //if (timemanager.Value <= 0)
        //{
        //    //GameOver();
        //}
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

        //Time.timeScale = 0f;

        //UIを表示させない

        //リザルト

        //Sceneリセット　ゲームリセット
        //SceneMove Tile

        m_gameOverUIEvent.Raise(true);

        //titel or restart
    }

}