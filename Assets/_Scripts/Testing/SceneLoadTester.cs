using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTester : MonoBehaviour
{
    public void LoadSettingsMenu()
    {
        StartCoroutine(AdditiveSceneManager.instance.LoadSceneAsync((int)AdditiveSceneManager.SceneIndices.SettingsMenuScene, LoadSceneMode.Additive));
    }
}