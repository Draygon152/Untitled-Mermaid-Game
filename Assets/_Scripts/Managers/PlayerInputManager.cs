using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : PersistentSingleton<PlayerInputManager>
{
    private PlayerInputControl playerInputControl;
    private bool settingsMenuOpened = false;



    protected override void Awake()
    {
        base.Awake();

        EventManager.instance.Subscribe(EventManager.EventTypes.SettingsMenuClosed, OnSettingsMenuClosedExternally);

        playerInputControl = new PlayerInputControl();
        playerInputControl.PlayerControls.Enable();
        playerInputControl.PlayerControls.Esc.started += (context) =>
        {
            if (settingsMenuOpened == false)
            {
                OpenSettingsMenu();
            }
            
            else
            {
                CloseSettingsMenu();
            }
        };
    }

    public Vector2 GetMouseAxisVector()
    {
        return playerInputControl.PlayerControls.MouseAxis.ReadValue<Vector2>();    
    }

    private void OpenSettingsMenu()
    {
        Time.timeScale = 0;
        settingsMenuOpened = true;
        StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync( (int)PersistentSceneManager.SceneIndices.SettingsMenuScene,
                                                                       LoadSceneMode.Additive) );
    }

    private void CloseSettingsMenu()
    {
        Time.timeScale = 1;
        settingsMenuOpened = false;
        StartCoroutine(PersistentSceneManager.instance.UnloadSceneAsync( (int)PersistentSceneManager.SceneIndices.SettingsMenuScene) );
    }

    private void OnSettingsMenuClosedExternally()
    {
        Time.timeScale = 1;
        settingsMenuOpened = false;
    }

    protected override void OnDestroy()
    {
        playerInputControl.PlayerControls.Disable();
        EventManager.instance.Unsubscribe(EventManager.EventTypes.SettingsMenuClosed, OnSettingsMenuClosedExternally);

        base.OnDestroy();
    }
}