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

    private bool m_isGameOver = false;
    public bool IsGameOver => m_isGameOver;


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

        if(!GameManager.Instance.ModifyMoney(5000))
        {
            Debug.LogError("ModifyMoney over ");
        }
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

        //m_isGameOver = true;

        //リザルト表示、関数を呼ぶ
        //if (m_level.Value % 5 == 0)
        //{
        //    m_gameClearUIEvent.Raise(true);
        //    m_gameClearEvent.Raise();

        //    //m_level.AddValue(1);

        //    return;

        //}

        //クリア階層記録　
        GameManager.Instance.AddLevel(1);
        //m_level.AddValue(1);
        //Debug.Log($"クリア回数：{m_level.Value}");
        //Debug.Log($"{m_level.name} : {m_level.Value}  InstanceID={m_level.GetInstanceID()}");



        

        //m_sceneEvent.TriggerEvent(m_mapPhaseScene);

        //for example
        //player do not move, state change, save, result
        //if (m_gameClearEvent != null)
        //{
        //    m_gameClearEvent.Raise();

        //}

        //return;

        //m_sceneEvent.TriggerEvent(m_mapPhaseScene);

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

        

        //Sceneリセット　ゲームリセット
        //SceneMove Tile

        m_gameOverUIEvent.Raise(true);

        //titel or restart
    }

}