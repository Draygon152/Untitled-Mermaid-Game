using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     Responsible for handling async loading/unloading of scenes
/// </summary>
public class PersistentSceneManager : LLPersistentSingleton<PersistentSceneManager>
{
    // Scene order should match build order in Build Settings
    public enum SceneIndices
    {
        MainMenuScene,
        SettingsMenuScene,
        GameScene,
        TrashyTrouble,
        FishyFreedom,
        PlanktonPlatoon,
        AlgaeAffliction,
        EndScene
    }



    public IEnumerator LoadSceneAsync(int sceneIndex, LoadSceneMode mode = LoadSceneMode.Single, Action action = null)
    {
        if (sceneIndex < 0 || sceneIndex > SceneManager.sceneCountInBuildSettings - 1)
        {
            Debug.LogError($"[PersistentSceneManager Error]: Provided scene index '{sceneIndex}' is not a valid scene index, is the scene in the build order?");
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, mode);
        asyncLoad.completed += (a) => { ActionExtensions.InvokeNullCheck(action); };

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public IEnumerator UnloadSceneAsync(int sceneIndex, Action action = null)
    {
        if (!SceneIsLoaded(sceneIndex))
        {
            Debug.LogError($"[PersistentSceneManager Error]: Provided scene '{Enum.GetName(typeof(SceneIndices), sceneIndex)}' is not currently loaded");
        }

        if (sceneIndex < 0 || sceneIndex > SceneManager.sceneCountInBuildSettings - 1)
        {
            Debug.LogError($"[PersistentSceneManager Error]: Provided scene index '{sceneIndex}' is not a valid scene index, is the scene in the build order?");
        }

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneIndex);
        asyncUnload.completed += (a) => { ActionExtensions.InvokeNullCheck(action); };

        while (!asyncUnload.isDone)
        {
            yield return null;
        }
    }

    public bool SceneIsLoaded(int sceneIndex)
    {
        return SceneManager.GetSceneByBuildIndex(sceneIndex).isLoaded;
    }
}