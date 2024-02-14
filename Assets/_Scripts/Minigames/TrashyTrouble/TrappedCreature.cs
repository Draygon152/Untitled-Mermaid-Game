using System;
using UnityEngine;

/// <summary>
///     Component script to control trapped creature behavior within minigame
/// </summary>

public class TrappedCreature : MonoBehaviour
{
    [SerializeField] private DraggableObject creature = null;

    [SerializeField] private RectTransform trapRect = null;
    [SerializeField] private SpriteRenderer trapSR = null;

    [SerializeField] private float fadeStartDelay = 0f;
    [SerializeField] private float fadeDuration = 0.4f;

    private bool creatureFreed = false;
    private bool isBeingReset = false;



    private void FixedUpdate()
    {
        // When draggable creature is freed
        if (!isBeingReset && !creatureFreed && !creature.rect.Overlaps(trapRect))
        {
            creatureFreed = true;
            FreeCreature();
        }
    }

    private Coroutine FreeCreature()
    {
        creature.ToggleDrag(false);

        Action<float> tweenAction = lerp =>
        {
            creature.spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(1f, 0f, lerp));
            trapSR.color = new Color(1, 1, 1, Mathf.Lerp(1f, 0f, lerp));
        };

        return this.DoTween(tweenAction,
                            () =>
                            {
                                gameObject.SetActive(false);
                                EventManager.instance.Notify(EventManager.EventTypes.CreatureFreed);
                            },
                            fadeDuration,
                            fadeStartDelay,
                            EaseType.linear,
                            true);
    }

    public void ResetCreature()
    {
        creature.spriteRenderer.color = new Color(1, 1, 1, 1);
        trapSR.color = new Color(1, 1, 1, 1);

        creatureFreed = false;
        isBeingReset = true;
        creature.ResetObject();
        gameObject.SetActive(true);
        isBeingReset = false;
    }
}