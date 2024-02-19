using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueAdvancer : SceneSingleton<DialogueAdvancer>
{
    [SerializeField] List<Sprite> panels = null;
    [SerializeField] Image displayPanel = null;
    [Space]
    [SerializeField] private bool intro = true;
    private int curPanel = 0;



    private void Start()
    {
        displayPanel.sprite = panels[curPanel];
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
}