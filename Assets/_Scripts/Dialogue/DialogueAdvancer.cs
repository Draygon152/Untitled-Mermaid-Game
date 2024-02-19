using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueAdvancer : SceneSingleton<DialogueAdvancer>
{
    [SerializeField] List<Sprite> panels = null;
    [SerializeField] Image displayPanel = null;
    [SerializeField] Button panelButton = null;
    [Space]
    [SerializeField] private bool intro = true;

    private int curPanel = 0;

    private AudioSource source = null;



    private void Start()
    {
        source = AudioManager.instance._sourceSFX;
        displayPanel.sprite = panels[curPanel];

        Cursor.SetCursor(GameManager.instance.defaultCursor, GameManager.instance.hotSpot, GameManager.instance.cursorMode);
    }

    public void AdvanceDialogue()
    {
        curPanel++;

        if (curPanel == panels.Count && intro)
        {
            StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync( (int)PersistentSceneManager.SceneIndices.GameScene,
                                                                           LoadSceneMode.Single));
        }

        else if (curPanel == panels.Count && !intro)
        {
            StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync( (int)PersistentSceneManager.SceneIndices.MainMenuScene,
                                                                           LoadSceneMode.Single));
        }

        else
        {
            displayPanel.sprite = panels[curPanel];
        }
    }

    protected override void OnDestroy()
    {
        panelButton.onClick.RemoveAllListeners();

        base.OnDestroy();
    }
}