using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueAdvancer : SceneSingleton<DialogueAdvancer>
{
    [SerializeField] List<Sprite> panels = null;
    [SerializeField] Image displayPanel = null;
    private int curPanel = 0;

    //private AudioSource source = null; 

    private void Start()
    {
        //source = AudioManager.instance._sourceSFX; 
        displayPanel.sprite = panels[curPanel];
    }


    
    public void AdvanceDialogue()
    {
        curPanel++;

        if (curPanel == panels.Count)
        {
            StartCoroutine(PersistentSceneManager.instance.LoadSceneAsync( (int)PersistentSceneManager.SceneIndices.GameScene,
                                                                           LoadSceneMode.Single));
        }

        else
        {
            displayPanel.sprite = panels[curPanel];
        }
    }
}