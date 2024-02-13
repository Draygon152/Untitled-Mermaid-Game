using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTester : MonoBehaviour
{
    public void LoadSettingsMenu()
    {
        StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync((int)PersistentSceneManager.SceneIndices.SettingsMenuScene, LoadSceneMode.Additive));
    }
}