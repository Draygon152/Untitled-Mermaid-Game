using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTester : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(MenuManager.instance.LoadSceneAsync((int)MenuManager.SceneIndices.SettingsMenu, LoadSceneMode.Additive));
    }
}