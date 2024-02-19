using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
///     <para>
///         Menu controller for the Main Menu
///     </para>
///     
///     SHOULD NOT BE INSTANTIATED VIA CODE, PREFAB FOR SETTINGS MENU SHOULD ONLY LIVE WITHIN ITS OWN SCENE
/// </summary>
public class MainMenu : MenuBase
{
    [SerializeField] private Button playButton = null;
    [SerializeField] private Button settingsButton = null;
    [SerializeField] private Button creditButton = null;
    [SerializeField] private Button backButton = null;


    [SerializeField] public Texture2D defaultCursor;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    [SerializeField] GameObject Panel;
    [SerializeField] GameObject MainMenuRoot;
    [SerializeField] GameObject ButtonBack;

    protected override void Start()
    {
        base.Start();

        Cursor.SetCursor(defaultCursor, hotSpot, cursorMode);
        AudioManager.instance.PlayMusic(AudioManager.instance._sourceMusic, AudioManager.instance.menuMusic);

        MainMenuRoot.SetActive(true);
        ButtonBack.SetActive(false);
    }

    public override void Init()
    {
        base.Init();

        playButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance._sourceSFX, AudioManager.instance.button1);

            // Load into main game scene
            //StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync( (int)PersistentSceneManager.SceneIndices.GameScene,
            //                                                               LoadSceneMode.Single));

            StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync( (int)PersistentSceneManager.SceneIndices.IntroScene,
                                                                           LoadSceneMode.Single));
        } );

        settingsButton.onClick.AddListener( () =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance._sourceSFX, AudioManager.instance.button2);

            StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync( (int)PersistentSceneManager.SceneIndices.SettingsMenuScene,
                                                                           LoadSceneMode.Additive));
        } );

        creditButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance._sourceSFX, AudioManager.instance.button2);

            Panel.SetActive(true);
            MainMenuRoot.SetActive(false);
            ButtonBack.SetActive(true);
        });

        backButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance._sourceSFX, AudioManager.instance.button2);

            Panel.SetActive(false);
            MainMenuRoot.SetActive(true);
            ButtonBack.SetActive(false);
        }

            );
        
    }

    private void OnDestroy()
    {
        settingsButton.onClick.RemoveAllListeners();
    }
}