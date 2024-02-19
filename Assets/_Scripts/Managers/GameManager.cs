using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     Main game management class, responsible for starting minigames in the main game scene
///     and keeping track of overall progress
/// </summary>
public class GameManager : PersistentSingleton<GameManager>
{
    private PersistentSceneManager.SceneIndices currentMinigame = PersistentSceneManager.SceneIndices.TrashyTrouble;

    private int score = 0;
    private bool reachedGameEnd = false;

    [SerializeField] public Texture2D defaultCursor;
    [SerializeField] public Texture2D HandCursor;
    [SerializeField] public Texture2D HandGrabCursor;
    [SerializeField] public Texture2D ScrubberCursor;
    [SerializeField] public Texture2D ScrubbingCursor;

    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero; 

    private void Start()
    {
        Cursor.SetCursor(defaultCursor, hotSpot, cursorMode);

        EventManager.instance.Subscribe(EventManager.EventTypes.MinigameEnd, ContinueToNextMinigame);
        StartMinigame(currentMinigame);
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

            case PersistentSceneManager.SceneIndices.EndScene:
                AudioManager.instance.PlayMusic(AudioManager.instance._sourceMusic, AudioManager.instance.endMusic);
                reachedGameEnd = true;
                break;

            default:
                Debug.LogError($"[GameManager Error]: Invalid scene index '{(int)sceneIndex}' provided");
                break;
        }

        if (!reachedGameEnd)
        {
            StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync((int)sceneIndex, LoadSceneMode.Additive));
        }

        else
        {
            StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync((int)sceneIndex, LoadSceneMode.Single, () => { GameCameraManager.instance.DisableCamera(); }));
        }
    }

    private void ContinueToNextMinigame()
    {
        currentMinigame++;
        StartMinigame(currentMinigame);
    }

    public void IncrementScore(int scoreIncrease)
    {
        score += scoreIncrease;
        Debug.Log($"New Score: {score}");
    }

    protected override void OnDestroy()
    {
        EventManager.instance.Unsubscribe(EventManager.EventTypes.MinigameEnd, ContinueToNextMinigame);

        base.OnDestroy();
    }
}