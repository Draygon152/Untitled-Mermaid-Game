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
    [Space]
    private AudioSource source = null;
    [SerializeField] private AudioClip happyClip = null;
    [SerializeField] private AudioClip sadClip = null;



    private void Start()
    {
        source = AudioManager.instance._sourceSFX;
        coralSpriteRenderer.sprite = coralNeutral;
    }

    public void Init()
    {
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
            AudioManager.instance.PlaySFX(source, happyClip);
        }
    }

    public void KillCoral()
    {
        coralSpriteRenderer.sprite = coralDead;
        AudioManager.instance.PlaySFX(source, sadClip);
        foreach (Scrubbable algae in algaeList)
        {
            algae.gameObject.SetActive(false);
        }
    }
}