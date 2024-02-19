using System;
using UnityEngine;

/// <summary>
///     Component script to control trapped creature behavior within minigame
/// </summary>
public class TrappedCreature : MonoBehaviour
{
    [SerializeField] private DraggableObject _creature = null;
    public DraggableObject creature => _creature;

    [SerializeField] private RectTransform trapRect = null;
    [SerializeField] private SpriteRenderer _trapSR = null;
    public SpriteRenderer trapSR => _trapSR;

    [SerializeField] private float fadeStartDelay = 0f;
    [SerializeField] private float fadeDuration = 0.4f;

    private bool creatureFreed = false;
    private bool isBeingReset = false;


    private AudioSource source = null;
    [SerializeField] AudioClip fishHappy;

    private void Start()
    {
        source = AudioManager.instance._sourceSFX;
    }

    private void FixedUpdate()
    {
        // When draggable creature is freed
        if (!isBeingReset && !creatureFreed && !_creature.rect.Overlaps(trapRect))
        {
            creatureFreed = true;
            AudioManager.instance.PlaySFX(source, fishHappy); 
            FreeCreature();
        }
    }

    private Coroutine FreeCreature()
    {
        _creature.ToggleDrag(false);
        EventManager.instance.Notify(EventManager.EventTypes.CreatureFreed);

        Action<float> tweenAction = lerp =>
        {
            _creature.spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(1f, 0f, lerp));
            _trapSR.color = new Color(1, 1, 1, Mathf.Lerp(1f, 0f, lerp));
        };

        return this.DoTween(tweenAction,
                            () =>
                            {
                                gameObject.SetActive(false);
                            },
                            fadeDuration,
                            fadeStartDelay,
                            EaseType.linear,
                            true);
    }

    public void ResetCreature()
    {
        _creature.spriteRenderer.color = new Color(1, 1, 1, 1);
        _trapSR.color = new Color(1, 1, 1, 1);

        creatureFreed = false;
        isBeingReset = true;
        _creature.ResetObject();
        gameObject.SetActive(true);
        isBeingReset = false;
    }
}