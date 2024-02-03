using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : LLPersistentSingleton<MenuManager>
{
    public enum SceneIndices
    {
        MainScene,
        SettingsMenu
    }

    public IEnumerator LoadSceneAsync(int sceneIndex, LoadSceneMode mode = LoadSceneMode.Single)
    {
        if (sceneIndex < 0 || sceneIndex > SceneManager.sceneCountInBuildSettings - 1)
        {
            Debug.LogError($"[MenuManager Error]: Provided scene index '{sceneIndex}' is not a valid scene index");
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, mode);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}