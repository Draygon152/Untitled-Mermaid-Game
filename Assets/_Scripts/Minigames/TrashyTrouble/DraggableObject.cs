using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///     Component script to be attached to 2D objects, allowing them to be draggable
/// </summary>

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(BoxCollider2D))]
[DisallowMultipleComponent]
public class DraggableObject : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform _rect = null;
    public RectTransform rect => _rect;

    [SerializeField] private SpriteRenderer _spriteRenderer = null;
    public SpriteRenderer spriteRenderer => _spriteRenderer;

    [Space]
    [Header("Movement Enforcement")]
    [SerializeField] private Vector2 movementDir = new Vector2(1, 0);
    [SerializeField] private bool enforceDirection = false;
    [SerializeField] private float dragSpeedModifier = 1f;
    private Vector2 startingPos = new Vector2(0, 0);
    
    private bool isDraggable = true;
    private bool isBeingDragged = false;
    private bool isFocused = true; // TODO: SET FALSE




    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isDraggable && eventData.button == PointerEventData.InputButton.Left)
        {
            isBeingDragged = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (enforceDirection && movementDir.x == 0 && movementDir.y == 0)
        {
            Debug.LogError($"[DraggableObject Error]: Cannot enforce a drag direction of (0, 0)");
        }

        if (isDraggable && isFocused && isBeingDragged)
        {
            float distance = Vector2.Dot(eventData.delta, movementDir) / (movementDir.magnitude * dragSpeedModifier);
            Vector3 projectedPos = new Vector3(_rect.localPosition.x + (movementDir.normalized.x * distance),
                                               _rect.localPosition.y + (movementDir.normalized.y * distance));

            // Enforce bounds so that object cannot be dragged past its starting position in the wrong direction
            if (enforceDirection)
            {
                if (movementDir.x > 0 && projectedPos.x > startingPos.x)
                {
                    if (movementDir.y > 0 && projectedPos.y > startingPos.y)
                    {
                        _rect.localPosition = projectedPos;
                    }

                    else if (movementDir.y < 0 && projectedPos.y < startingPos.y)
                    {
                        _rect.localPosition = projectedPos;
                    }

                    else if (movementDir.y == 0)
                    {
                        _rect.localPosition = projectedPos;
                    }
                }

                else if (movementDir.x < 0 && projectedPos.x < startingPos.x)
                {
                    if (movementDir.y > 0 && projectedPos.y > startingPos.y)
                    {
                        _rect.localPosition = projectedPos;
                    }

                    else if (movementDir.y < 0 && projectedPos.y < startingPos.y)
                    {
                        _rect.localPosition = projectedPos;
                    }

                    else if (movementDir.y == 0 && projectedPos.x < startingPos.x)
                    {
                        _rect.localPosition = projectedPos;
                    }
                }

                else if (movementDir.x == 0)
                {
                    if (movementDir.y > 0 && projectedPos.y > startingPos.y)
                    {
                        _rect.localPosition = projectedPos;
                    }

                    else if (movementDir.y < 0 && projectedPos.y < startingPos.y)
                    {
                        _rect.localPosition = projectedPos;
                    }
                }
            }

            else
            {
                _rect.localPosition = projectedPos;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDraggable && eventData.button == PointerEventData.InputButton.Left)
        {
            isBeingDragged = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void ToggleFocus(bool argValue)
    {
        isFocused = argValue;
    }

    public void ToggleDrag(bool argValue)
    {
        isDraggable = argValue;
    }

    public void ResetObject()
    {
        isDraggable = true;
        isBeingDragged = false;
        isFocused = true; // TODO: SET FALSE

        _rect.position = startingPos;
    }
}