using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Coral : MonoBehaviour
{
    [SerializeField] private List<Scrubbable> algaeList = null;
    private int algaeScrubbed = 0;
    [Space]
    [SerializeField] private SpriteRenderer coralSpriteRenderer = null;
    [Space]
    [SerializeField] private Sprite coralNeutral = null;
    [SerializeField] private Sprite coralHappy = null;
    [SerializeField] private Sprite coralDead = null;



    private void Start()
    {
        coralSpriteRenderer.sprite = coralNeutral;

        foreach (Scrubbable algae in algaeList)
        {
            algae.Init(OnAlgaeScrubbed);
        }
    }

    public void OnAlgaeScrubbed()
    {
        algaeScrubbed++;

        if (algaeScrubbed == algaeList.Count)
        {
            EventManager.instance.Notify(EventManager.EventTypes.CoralCleaned);
            coralSpriteRenderer.sprite = coralHappy;
        }
    }

    public void KillCoral()
    {
        coralSpriteRenderer.sprite = coralDead;
        foreach (Scrubbable algae in algaeList)
        {
            algae.gameObject.SetActive(false);
        }
    }
}