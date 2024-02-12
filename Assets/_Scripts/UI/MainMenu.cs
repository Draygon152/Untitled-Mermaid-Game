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



    public override void Init()
    {
        base.Init();

        playButton.onClick.AddListener(() =>
        {
            // Load into main game scene
            StartCoroutine(AdditiveSceneManager.instance.LoadSceneAsync((int)AdditiveSceneManager.SceneIndices.GameScene,
                                                                         LoadSceneMode.Single,
                                                                         () => { AudioManager.instance.StartMusic(null, AudioManager.instance.planktonMusic); } ));
        } );

        settingsButton.onClick.AddListener( () =>
        {
            StartCoroutine(AdditiveSceneManager.instance.LoadSceneAsync( (int)AdditiveSceneManager.SceneIndices.SettingsMenuScene,
                                                                         LoadSceneMode.Additive));
        } );
    }

    private void OnDestroy()
    {
        settingsButton.onClick.RemoveAllListeners();
    }
}