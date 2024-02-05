using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MenuBase<MainMenu>
{
    [SerializeField] private Button playButton = null;
    [SerializeField] private Button settingsButton = null;



    protected override void Awake()
    {
        base.Awake();
    }

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
                                                                         LoadSceneMode.Additive,
                                                                         () =>
                                                                         {
                                                                             SettingsMenu.instance.Init();
                                                                         }));
        } );
    }

    protected override void OnDestroy()
    {
        settingsButton.onClick.RemoveAllListeners();
        base.OnDestroy();
    }
}