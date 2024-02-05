using UnityEngine;
using UnityEngine.UI;

// Singleton responsible for handling SettingsMenu functionality
//
// SHOULD NOT BE INSTANTIATED IN CODE, PREFAB FOR SETTINGS MENU SHOULD ONLY LIVE WITHIN ITS OWN SCENE
public class SettingsMenu : SceneSingleton<SettingsMenu>
{
    [SerializeField] private Button _videoButton = null;
    [SerializeField] private Button _audioButton = null;
    [SerializeField] private Button _returnButton = null;



    public void Init()
    {
        _returnButton.onClick.AddListener(delegate { StartCoroutine(MenuManager.instance.UnloadSceneAsync((int)MenuManager.SceneIndices.SettingsMenuScene)); } );
    }

    protected override void OnDestroy()
    {
        _returnButton.onClick.RemoveAllListeners();
        base.OnDestroy();
    }
}