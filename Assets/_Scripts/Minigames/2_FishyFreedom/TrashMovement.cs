using UnityEngine;

public class TrashMovement : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float snapSpeed = 5f;
    [Space]
    [SerializeField] private Collider2D mainCollider = null;

    private Transform _hook = null;
    public Transform hook => _hook;

    private bool minigameOver = false;
    private bool isDragging = false;
    private bool _caught = false;
    public bool caught => _caught;

    private Vector2 initialPosition;

    private AudioSource source = null;
    [SerializeField] AudioClip trashCaught;



    private void Start()
    {
        source = AudioManager.instance._sourceSFX;

        initialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (!minigameOver)
        {
            if (!_caught && !isDragging)
            {
                MoveOffScreen();
            }

            else if (_caught)
            {
                if (!_hook.gameObject.activeInHierarchy)
                {
                    gameObject.SetActive(false);
                    transform.position = initialPosition;
                    _caught = false;
                    mainCollider.enabled = true;

                    return;
                }

                if (Mathf.Abs(transform.position.y - _hook.transform.position.y) < 0.1f)
                {
                    transform.position = new Vector2(_hook.position.x, _hook.position.y);
                }

                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, _hook.position, snapSpeed * Time.fixedDeltaTime);
                }
            }

            else if (isDragging)
            {
                Vector3 mousePosition = FishyFreedomManager.instance.canvas.worldCamera.ScreenToWorldPoint(PlayerInputManager.instance.GetMouseAxisVector());
                transform.position = new Vector2(mousePosition.x, mousePosition.y);
            }
        }
    }

    private void MoveOffScreen()
    {
        float direction = transform.localScale.x > 0 ? 1 : -1;

        Vector2 moveDirection = new Vector2(speed * direction, 0);
        float floatingEffect = Mathf.Sin(Time.fixedTime) * 0.3f;
        transform.position += new Vector3(moveDirection.x, moveDirection.y + floatingEffect, 0) * Time.fixedDeltaTime;
    }
    
    private void StartDragging()
    {
        isDragging = true;

        Cursor.SetCursor(GameManager.instance.HandGrabCursor, GameManager.instance.hotSpot, GameManager.instance.cursorMode);
    }

    public void OnCaught(Transform hookTransform)
    {
        _caught = true;

        AudioManager.instance.PlaySFX(source, trashCaught);

        _hook = hookTransform;
        mainCollider.enabled = false;
    }

    public void OnMinigameOver()
    {
        minigameOver = true;
    }

    private void OnMouseDown()
    {
        StartDragging();
    }

    private void OnMouseUp()
    {
        isDragging = false;
        mainCollider.enabled = true;

        Cursor.SetCursor(GameManager.instance.HandCursor, GameManager.instance.hotSpot, GameManager.instance.cursorMode);
    }

    private void OnBecameInvisible()
    {
        if (_caught)
        {
            EventManager.instance.Notify(EventManager.EventTypes.TrashPulledUp);
        }

        gameObject.SetActive(false);
        transform.position = initialPosition;
        _caught = false;
        mainCollider.enabled = true;
    }
}