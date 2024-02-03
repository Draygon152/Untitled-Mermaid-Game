using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : LLPersistentSingleton<MenuManager>
{
    public IEnumerator LoadSceneAsync(Scene scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.buildIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}