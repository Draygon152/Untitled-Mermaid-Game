using UnityEngine.SceneManagement;

/// <summary>
///     Main game management class
/// </summary>
public class GameManager : PersistentSingleton<GameManager>
{
    private void Start()
    {
        StartMinigame(PersistentSceneManager.SceneIndices.TrashyTrouble);
    }

    public void StartMinigame(PersistentSceneManager.SceneIndices sceneIndex)
    {
        if (sceneIndex == PersistentSceneManager.SceneIndices.TrashyTrouble)
        {
            AudioManager.instance.PlayMusic(AudioManager.instance._sourceMusic, AudioManager.instance.trashyTroubleMusic);
            StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync( (int)sceneIndex, LoadSceneMode.Additive ));
        }

        else if (sceneIndex == PersistentSceneManager.SceneIndices.FishyFreedom)
        {
            AudioManager.instance.PlayMusic(AudioManager.instance._sourceMusic, AudioManager.instance.overFishingMusic);
        }
        
        else if (sceneIndex == PersistentSceneManager.SceneIndices.PlanktonPlatoon)
        {
            AudioManager.instance.PlayMusic(AudioManager.instance._sourceMusic, AudioManager.instance.planktonMusic);
        }
        
        else if (sceneIndex == PersistentSceneManager.SceneIndices.AlgaeAffliction)
        {
            AudioManager.instance.PlayMusic(AudioManager.instance._sourceMusic, AudioManager.instance.coralCleaningMusic);
        }
    }
}