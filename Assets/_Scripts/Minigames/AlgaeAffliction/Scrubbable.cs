using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private RectTransform scrubbableRect = null;
    [SerializeField] private SpriteRenderer scrubbableRenderer = null;
    [Space]
    [Range(0.01f, 0.1f)]
    [SerializeField] private float scrubSpeedModifier = 0.1f;

    private bool isScrubbable = true;
    private bool isBeingScrubbed = false;

    private bool scrubbedOut = false;
    private bool isBeingReset = false;



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
                EventManager.instance.Notify(EventManager.EventTypes.AlgaeScrubbed);
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
        if (!isBeingReset && isScrubbable && eventData.button == PointerEventData.InputButton.Left)
        {
            isBeingScrubbed = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isBeingReset && isScrubbable && isBeingScrubbed)
        {
            float scrubAmount = Mathf.Abs(eventData.delta.magnitude) * scrubSpeedModifier;

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