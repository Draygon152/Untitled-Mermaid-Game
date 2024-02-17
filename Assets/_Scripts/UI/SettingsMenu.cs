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
    [SerializeField] private CanvasGroup audioSubMenuCG = null;

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

    private void OnDestroy()
    {
        audioButton.onClick.RemoveAllListeners();
        returnButton.onClick.RemoveAllListeners();
    }
}