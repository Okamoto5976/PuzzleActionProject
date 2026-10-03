using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [SerializeField] private StaticSceneAsset m_scene;

    [SerializeField] private GameObject m_option;

    [SerializeField] private Slider m_masterSlider;
    [SerializeField] private Slider m_bgmSlider;
    [SerializeField] private Slider m_seSlider;

    private InventorySaveManager m_inventorySaveManager;
    private PlayerSave m_playerSave;

    [SerializeField] private GameObject m_tutorial;

    //-----audio save set-------------
    private OptionSaveManager m_optionSaveManager = new();

    private void Awake()
    {
        m_inventorySaveManager = GetComponent<InventorySaveManager>();
        m_playerSave = new();
    }

    private void Start()
    {
        var data = m_optionSaveManager.OnAudioLoad();

        if (data != null)
        {
            m_masterSlider.value = data.m_masterVolume;
            m_bgmSlider.value = data.m_bgmVolume;
            m_seSlider.value = data.m_seVolume;
        }
        else
        {
            m_masterSlider.value = 0.8f;
            m_bgmSlider.value = 0.8f;
            m_seSlider.value = 0.8f;
        }

    }

    public void SetMasterVolume(float value)
    {
        AudioManager.Instance.SetMaster(value);
    }

    public void SetBGMVolume(float value)
    {
        AudioManager.Instance.SetBGM(value);
    }

    public void SetSEVolume(float value)
    {
        AudioManager.Instance.SetSE(value);
    }

    public void OnStart()
    {
        if(GameManager.Instance.m_isTutorial)
        {
            m_tutorial.SetActive(true);
        }
        else
        {
            GameStart();
        }
    }

    private void GameStart()
    {
        GameManager.Instance.SetLevel(1);
        //save reset
        GameManager.Instance.SetMoney(0);

        m_inventorySaveManager.ClearSaveData();
        m_playerSave.DeletePlayerData();

        LoadManager.m_instance.LoadScene(m_scene.Value);
    }
    

    public void OnOption(bool isActive)
    {
        m_option.SetActive(isActive);
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
