using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MenuBase
{
    [SerializeField] private Button playButton = null;
    [SerializeField] private Button settingsButton = null;



    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

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