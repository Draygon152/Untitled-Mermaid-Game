using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     Responsible for handling async loading/unloading of scenes
/// </summary>
public class AdditiveSceneManager : LLPersistentSingleton<AdditiveSceneManager>
{
    // Scene order should match build order in Build Settings
    public enum SceneIndices
    {
        MainScene,
        SettingsMenuScene
    }



    public IEnumerator LoadSceneAsync(int sceneIndex, LoadSceneMode mode = LoadSceneMode.Single, Action action = null)
    {
        if (sceneIndex < 0 || sceneIndex > SceneManager.sceneCountInBuildSettings - 1)
        {
            Debug.LogError($"[AdditiveSceneManager Error]: Provided scene index '{sceneIndex}' is not a valid scene index, is the scene in the build order?");
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, mode);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        ActionExtensions.InvokeNullCheck(action);
    }

    public IEnumerator UnloadSceneAsync(int sceneIndex, Action action = null)
    {
        if (!SceneIsLoaded(sceneIndex))
        {
            Debug.LogError($"[AdditiveSceneManager Error]: Provided scene '{Enum.GetName(typeof(SceneIndices), sceneIndex)}' is not currently loaded");
        }

        if (sceneIndex < 0 || sceneIndex > SceneManager.sceneCountInBuildSettings - 1)
        {
            Debug.LogError($"[AdditiveSceneManager Error]: Provided scene index '{sceneIndex}' is not a valid scene index, is the scene in the build order?");
        }

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneIndex);

        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        ActionExtensions.InvokeNullCheck(action);
    }

    public bool SceneIsLoaded(int sceneIndex)
    {
        return SceneManager.GetSceneByBuildIndex(sceneIndex).isLoaded;
    }
}