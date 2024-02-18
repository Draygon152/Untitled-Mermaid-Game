using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     <para>
///         Menu controller for the Settings Menu
///     </para>
///     
///     SHOULD NOT BE INSTANTIATED VIA CODE, PREFAB FOR SETTINGS MENU SHOULD ONLY LIVE WITHIN ITS OWN SCENE
/// </summary>
public class SettingsMenu : MenuBase
{
    [SerializeField] private Button videoButton = null;
    [SerializeField] private Button audioButton = null;
    [SerializeField] private Button returnButton = null;

    [SerializeField] private CanvasGroup videoSubMenuCG = null;
    [Space]
    [SerializeField] private CanvasGroup audioSubMenuCG = null;
    [SerializeField] private Slider masterVol = null;
    [SerializeField] private Slider sfxVol = null;
    [SerializeField] private Slider musicVol = null;

    private bool videoSubMenuOpen = false;
    private bool audioSubMenuOpen = false;



    public override void Init()
    {
        base.Init();

        videoSubMenuCG.alpha = 0f;
        audioSubMenuCG.alpha = 0f;

        audioButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance._sourceSFX, AudioManager.instance.button1);
            OpenAudioSubMenu();

        });

        returnButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySFX(AudioManager.instance._sourceSFX, AudioManager.instance.button2);

            // If audio submenu is open when return button is pressed, fade it out
            if (audioSubMenuCG.alpha > 0f)
            {
                FadeOut(audioSubMenuCG, fadeDuration, fadeStartDelay, EaseType.linear, () => { audioSubMenuOpen = false; });
            }

            FadeOut(mainCG, fadeDuration, fadeStartDelay, EaseType.linear, () =>
            {
                EventManager.instance.Notify(EventManager.EventTypes.SettingsMenuClosed);
                StartCoroutine(PersistentSceneManager.instance.UnloadSceneAsync( (int)PersistentSceneManager.SceneIndices.SettingsMenuScene) );
            } );
        });

        masterVol.onValueChanged.AddListener(OnMasterVolChanged);
        sfxVol.onValueChanged.AddListener(OnSFXVolChanged);
        musicVol.onValueChanged.AddListener(OnMusicVolChanged);
        LoadAudioSliderSettings();
    }

    public void OpenAudioSubMenu()
    {
        if (audioSubMenuOpen)
        {
            CloseAudioSubMenu();
            return;
        }
        audioSubMenuOpen = true;

        FadeIn(audioSubMenuCG, fadeDuration, fadeStartDelay, EaseType.linear);
    }

    public void CloseAudioSubMenu()
    {
        if (audioSubMenuOpen)
        {
            FadeOut(audioSubMenuCG, fadeDuration, fadeStartDelay, EaseType.linear);
 
            audioSubMenuOpen = false;
        }

        else
        {
            Debug.Log($"[SettingsMenu Error]: Audio SubMenu is not opened");
            return;
        }
    }

    private void LoadAudioSliderSettings()
    {
        masterVol.value = SaveDataUtility.LoadFloat(SaveDataUtility.MASTER_VOLUME, 0.5f);
        sfxVol.value = SaveDataUtility.LoadFloat(SaveDataUtility.SFX_VOLUME, 0.5f);
        musicVol.value = SaveDataUtility.LoadFloat(SaveDataUtility.MUSIC_VOLUME, 0.5f);
    }

    private void OnMasterVolChanged(float argValue)
    {
        SaveDataUtility.SaveFloat(SaveDataUtility.MASTER_VOLUME, argValue);
        AudioManager.instance.LoadMasterVolume();
    }

    private void OnSFXVolChanged(float argValue)
    {
        SaveDataUtility.SaveFloat(SaveDataUtility.SFX_VOLUME, argValue);
        AudioManager.instance.LoadSFXVolume();
    }
    
    private void OnMusicVolChanged(float argValue)
    {
        SaveDataUtility.SaveFloat(SaveDataUtility.MUSIC_VOLUME, argValue);
        AudioManager.instance.LoadMusicVolume();
    }

    private void OnDestroy()
    {
        audioButton.onClick.RemoveAllListeners();
        returnButton.onClick.RemoveAllListeners();

        masterVol.onValueChanged.RemoveAllListeners();
        sfxVol.onValueChanged.RemoveAllListeners();
        musicVol.onValueChanged.RemoveAllListeners();
    }
}