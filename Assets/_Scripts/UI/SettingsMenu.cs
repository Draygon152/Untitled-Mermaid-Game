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

        audioButton.onClick.AddListener(() => { OpenAudioSubMenu(); });

        returnButton.onClick.AddListener( () =>
        {
            // If audio submenu is open when return button is pressed, fade it out
            if (audioSubMenuCG.alpha > 0f)
            {
                FadeOut(audioSubMenuCG, fadeDuration, fadeStartDelay, EaseType.linear, () => { audioSubMenuOpen = false; });
            }

            FadeOut(mainCG, fadeDuration, fadeStartDelay, EaseType.linear, () =>
            {
                StartCoroutine(AdditiveSceneManager.instance.UnloadSceneAsync((int)AdditiveSceneManager.SceneIndices.SettingsMenuScene));
            } );
        } );
    }

    public void OpenAudioSubMenu()
    {
        if (audioSubMenuOpen)
        {
            Debug.Log($"[SettingsMenu Error]: Audio SubMenu is already opened");
            return;
        }
        audioSubMenuOpen = true;

        FadeIn(audioSubMenuCG, fadeDuration, fadeStartDelay, EaseType.linear);
    }

    private void OnDestroy()
    {
        audioButton.onClick.RemoveAllListeners();
        returnButton.onClick.RemoveAllListeners();
    }
}