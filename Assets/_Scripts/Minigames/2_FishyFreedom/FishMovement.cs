using System;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float visibilityRange = 50f;
    [SerializeField] private float snapSpeed = 50f;
    [SerializeField] private float rotSpeed = 10f;
    [SerializeField] private float fieldOfView = 45f;
    [Space]
    [SerializeField] private Collider2D mainCollider = null;

    private GameObject nearestHook = null;
    private Transform _hook = null;
    public Transform hook => _hook;

    private float checkRate = 0.3f;
    private float nextCheckTime = 0f;

    private bool isDragging = false;
    private bool _caught = false;
    public bool caught => _caught;
    
    private Vector2 moveDirection;
    private Vector2 initialPosition;
    private Quaternion initialRotation;
    


    private void Start()
    {
        initialRotation = transform.rotation;
        initialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (!_caught && !isDragging)
        {
            if (Time.fixedTime >= nextCheckTime)
            {
                nearestHook = FindNearestHook();
                nextCheckTime = Time.fixedTime + checkRate;
            }

            if (nearestHook != null)
            {
                Vector2 toHook = (nearestHook.transform.position - transform.position).normalized;
                Vector2 fishForward = transform.localScale.x < 0 ? transform.right.normalized : -transform.right.normalized;
                float angleToHook = Vector2.Angle(fishForward, toHook);

                if (angleToHook <= fieldOfView && Vector2.Distance(transform.position, nearestHook.transform.position) < visibilityRange)
                {
                    MoveTowardsHook();
                    return;
                }
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, Time.fixedDeltaTime * rotSpeed);   
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
            transform.position = new Vector2(transform.position.x, mousePosition.y);
        }
    }

    private void MoveTowardsHook()
    {
        float step = speed * Time.fixedDeltaTime;
        Vector3 hookPosition = nearestHook.transform.position;
        moveDirection = (hookPosition - transform.position).normalized;

        transform.position = Vector2.MoveTowards(transform.position, hookPosition, step);

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        angle += transform.localScale.x < 0 ? 0 : 180; 

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        Quaternion smoothRotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotSpeed);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 100) 
        {
            transform.rotation = smoothRotation;
        }
    }

    private void MoveOffScreen()
    {
        float direction = transform.localScale.x > 0 ? -1 : 1;

        moveDirection = new Vector2(speed * direction, 0);
        transform.position += new Vector3(moveDirection.x, moveDirection.y, 0) * Time.fixedDeltaTime;
    }

    private GameObject FindNearestHook()
    {
        GameObject[] hooks = GameObject.FindGameObjectsWithTag("Hook");
        GameObject closestHook = null;
        float minDistance = Mathf.Infinity;
        Vector2 currentPosition = transform.position;

        foreach (GameObject hook in hooks)
        {
            bool hooked = hook.GetComponentInParent<HookBehavior>().caughtFish;
            if (!hooked)
            {
                float distance = Vector2.Distance(hook.transform.position, currentPosition);
                if (distance < minDistance)
                {
                    closestHook = hook;
                    minDistance = distance;
                }
            }
        }
        return closestHook;
    }

    private void StartDragging()
    {
        isDragging = true;
        _caught = false;
    }

    public void OnCaught(Transform hookTransform)
    {
        _caught = true;
        _hook = hookTransform;
    }

    private void OnMouseDown()
    {
        StartDragging();
    }

    private void OnMouseUp()
    {
        isDragging = false;
        mainCollider.enabled = true;
    }

    private void OnBecameInvisible()
    {
        if (_caught)
        {
            EventManager.instance.Notify(EventManager.EventTypes.MinigameFail);
        }

        gameObject.SetActive(false);
        transform.position = initialPosition;
        _caught = false;
        mainCollider.enabled = true;
    }
}