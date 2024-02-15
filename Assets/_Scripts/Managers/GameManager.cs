using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     Main game management class, responsible for starting minigames in the main game scene
///     and keeping track of overall progress
/// </summary>
public class GameManager : PersistentSingleton<GameManager>
{
    private void Start()
    {
        StartMinigame(PersistentSceneManager.SceneIndices.TrashyTrouble);
    }

    public void StartMinigame(PersistentSceneManager.SceneIndices sceneIndex)
    {
        switch(sceneIndex)
        {
            case PersistentSceneManager.SceneIndices.TrashyTrouble:
                AudioManager.instance.PlayMusic(AudioManager.instance._sourceMusic, AudioManager.instance.trashyTroubleMusic);
                break;

            case PersistentSceneManager.SceneIndices.FishyFreedom:
                AudioManager.instance.PlayMusic(AudioManager.instance._sourceMusic, AudioManager.instance.overFishingMusic);
                break;

            case PersistentSceneManager.SceneIndices.PlanktonPlatoon:
                AudioManager.instance.PlayMusic(AudioManager.instance._sourceMusic, AudioManager.instance.planktonMusic);
                break;

            case PersistentSceneManager.SceneIndices.AlgaeAffliction:
                AudioManager.instance.PlayMusic(AudioManager.instance._sourceMusic, AudioManager.instance.coralCleaningMusic);
                break;

            default:
                Debug.LogError($"[GameManager Error]: Invalid scene index '{(int)sceneIndex}' provided");
                break;
        }

        StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync((int)sceneIndex, LoadSceneMode.Additive));
    }
}