using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///     Component script to be attached to 2D objects, allowing them to be "scrubbed" out of existence
/// </summary>

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(BoxCollider2D))]
[DisallowMultipleComponent]
public class Scrubbable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private SpriteRenderer scrubbableRenderer = null;
    [Space]
    [Range(0.1f, 1f)]
    [SerializeField] private float scrubSpeedModifier = 1f;
    private const int SCRUB_SPEED_DIVISOR = 1000;

    private bool isScrubbable = false;
    private bool isBeingScrubbed = false;

    private bool scrubbedOut = false;
    private bool isBeingReset = false;

    private Action onScrubbed = null;

    private AudioSource source = null;
    [SerializeField] private AudioClip scrubbingClip = null;



    private void Start()
    {
        source = AudioManager.instance._sourceSFX;
    }

    public void Init(Action onScrubbedCallback)
    {
        onScrubbed += onScrubbedCallback;
        isScrubbable = true;
    }

    private void ScrubObject(float amount)
    {
        if (!scrubbedOut)
        {
            float projectedAlpha = scrubbableRenderer.color.a - amount;
            if (projectedAlpha <= 0f)
            {
                scrubbableRenderer.color = new Color(1, 1, 1, 0);
                isScrubbable = false;
                scrubbedOut = true;
                AudioManager.instance.StopSFXLoop(source);
                onScrubbed.InvokeNullCheck();
                gameObject.SetActive(false);
            }

            else
            {
                scrubbableRenderer.color = new Color(1, 1, 1, projectedAlpha);
            }
        }
    }

    public void ResetScrubbable()
    {
        scrubbableRenderer.color = new Color(1, 1, 1, 1);

        isScrubbable = true;
        isBeingScrubbed = false;
        scrubbedOut = false;
        isBeingReset = false;
        gameObject.SetActive(true);
        isBeingReset = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        AudioManager.instance.PlaySFXLoop(source, scrubbingClip);

        Cursor.SetCursor(GameManager.instance.ScrubbingCursor, GameManager.instance.hotSpot, GameManager.instance.cursorMode);

        if (!isBeingReset && isScrubbable && eventData.button == PointerEventData.InputButton.Left)
        {
            isBeingScrubbed = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isBeingReset && isScrubbable && isBeingScrubbed)
        {
            float scrubAmount = Mathf.Abs(eventData.delta.magnitude) * (scrubSpeedModifier / SCRUB_SPEED_DIVISOR);

            ScrubObject(scrubAmount);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isBeingReset && isScrubbable && eventData.button == PointerEventData.InputButton.Left)
        {
            isBeingScrubbed = false;
        }
    }
}