using UnityEngine;
using UnityEngine.UI;

// Singleton responsible for handling SettingsMenu functionality
//
// SHOULD NOT BE INSTANTIATED IN CODE, PREFAB FOR SETTINGS MENU SHOULD ONLY LIVE WITHIN ITS OWN SCENE
public class SettingsMenu : MenuBase
{
    [SerializeField] private Button videoButton = null;
    [SerializeField] private Button audioButton = null;



    // Add return button listeners here
    public override void Init()
    {
        base.Init();
        returnButton.onClick.AddListener( () =>
        {
            FadeOut(mainCanvasGroup, fadeDuration, fadeStartDelay, EaseType.linear, () =>
            {
                StartCoroutine(AdditiveSceneManager.instance.UnloadSceneAsync((int)AdditiveSceneManager.SceneIndices.SettingsMenuScene));
            } );
        } );
    }
}