using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTester : MonoBehaviour
{
    public void LoadSettingsMenu()
    {
        StartCoroutine(MenuManager.instance.LoadSceneAsync((int)MenuManager.SceneIndices.SettingsMenuScene, LoadSceneMode.Additive, () => { SettingsMenu.instance.Init(); } ));
    }
}