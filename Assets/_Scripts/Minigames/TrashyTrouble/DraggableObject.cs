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

    [SerializeField] private Vector2 movementDir = new Vector2(1, 0);
    private Vector2 startingPos = new Vector2(0, 0);

    [SerializeField] private bool enforceDirection = false;
    private bool isDraggable = true;
    private bool isBeingDragged = false;
    private bool isFocused = true;



    private void Start()
    {
        startingPos = _rect.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isDraggable && eventData.button == PointerEventData.InputButton.Left)
        {
            isBeingDragged = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDraggable && isFocused && isBeingDragged)
        {
            float distance = Vector2.Dot(eventData.delta, movementDir) / movementDir.magnitude;
            Vector3 projectedPos = new Vector3(_rect.localPosition.x + (movementDir.normalized.x * distance),
                                               _rect.localPosition.y + (movementDir.normalized.y * distance));

            // Enforce bounds so that object cannot be dragged past its starting position in the wrong direction
            if (enforceDirection)
            {
                if (movementDir.x > 0)
                {
                    if (movementDir.y > 0)
                    {
                        if (projectedPos.x >= startingPos.x && projectedPos.y >= startingPos.y)
                        {
                            _rect.localPosition = projectedPos;
                        }
                    }

                    else if (movementDir.y < 0)
                    {
                        if (projectedPos.x >= startingPos.x && projectedPos.y <= startingPos.y)
                        {
                            _rect.localPosition = projectedPos;
                        }
                    }
                }

                else if (movementDir.x < 0)
                {
                    if (movementDir.y > 0)
                    {
                        if (projectedPos.x <= startingPos.x && projectedPos.y >= startingPos.y)
                        {
                            _rect.localPosition = projectedPos;
                        }
                    }

                    else if (movementDir.y < 0)
                    {
                        if (projectedPos.x <= startingPos.x && projectedPos.y <= startingPos.y)
                        {
                            _rect.localPosition = projectedPos;
                        }
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

    public void DisableDrag()
    {
        isDraggable = false;
    }
}