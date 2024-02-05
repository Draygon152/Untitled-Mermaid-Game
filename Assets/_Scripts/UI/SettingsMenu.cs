using UnityEngine;
using UnityEngine.UI;

// Singleton responsible for handling SettingsMenu functionality
//
// SHOULD NOT BE INSTANTIATED IN CODE, PREFAB FOR SETTINGS MENU SHOULD ONLY LIVE WITHIN ITS OWN SCENE
public class SettingsMenu : MenuBase
{
    [SerializeField] private Button videoButton = null;
    [SerializeField] private Button audioButton = null;

    [SerializeField] private CanvasGroup videoSubMenuCG = null;
    [SerializeField] private CanvasGroup audioSubMenuCG = null;

    private bool videoSubMenuOpen = false;
    private bool audioSubMenuOpen = false;



    // Add return button listeners here
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

    protected override void OnDestroy()
    {
        audioButton.onClick.RemoveAllListeners();
        base.OnDestroy();
    }
}