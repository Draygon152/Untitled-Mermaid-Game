using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///     Component script to be attached to 2D objects, allowing them to be draggable
/// </summary>

[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class DraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform rect = null;
    
    private bool isBeingDragged = false;
    private bool isFocused = false;



    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isBeingDragged = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isBeingDragged)
        {
            rect.position = new Vector3(rect.position.x + eventData.delta.x,
                                        rect.position.y);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
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
}