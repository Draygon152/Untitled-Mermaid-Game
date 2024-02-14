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



    protected override void Start()
    {
        base.Start();

        AudioManager.instance.PlayMusic(AudioManager.instance._sourceMusic, AudioManager.instance.storyMusic);
    }

    public override void Init()
    {
        base.Init();

        playButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance._sourceSFX, AudioManager.instance.button1);

            // Load into main game scene
            StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync((int)PersistentSceneManager.SceneIndices.GameScene,
                                                                         LoadSceneMode.Single));
        } );

        settingsButton.onClick.AddListener( () =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance._sourceSFX, AudioManager.instance.button2);

            StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync( (int)PersistentSceneManager.SceneIndices.SettingsMenuScene,
                                                                           LoadSceneMode.Additive));
        } );
    }

    private void OnDestroy()
    {
        settingsButton.onClick.RemoveAllListeners();
    }
}