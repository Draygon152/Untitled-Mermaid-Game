using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float visibilityRange = 5f;
    [SerializeField] private float snapSpeed = 5f;
    [SerializeField] private float rotSpeed = 1f;
    [SerializeField] float fieldOfView = 45f; 
    private GameObject nearestHook;
    private float checkRate = 0.3f;
    private float nextCheckTime = 0f;
    private bool isDragging = false;
    private Collider2D mainCollider;
    private Collider2D triggerCollider; 
    public bool caught = false;
    public Transform Hook;
    private Vector2 moveDirection;
    private Quaternion initialRotation;

    void Start()
    {
        var colliders = GetComponents<BoxCollider2D>();
        if (colliders.Length > 1)
        {
            mainCollider = colliders[0]; 
            triggerCollider = colliders[1];
        }
        else
        {
            Debug.LogError("Required colliders not found on Fish GameObject.");
        }
        if (transform.position.x < -10)
        {
            Vector3 newScale = transform.localScale;
            newScale.x = -newScale.x;
            transform.localScale = newScale;
        }
        initialRotation = transform.rotation;    
    }

    void Update()
    {
        if (transform.position.x < -12 || transform.position.x > 12)
        {
            Destroy(gameObject);
        }
        if (!caught && !isDragging)
        {
            if (Time.time >= nextCheckTime)
            {
                nearestHook = FindNearestHook();
                nextCheckTime = Time.time + checkRate;
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
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, Time.deltaTime * rotSpeed);   
            MoveOffScreen();
        }
        else if (caught) 
        {
            if (Hook == null) 
            {
                Destroy(gameObject);
                return;
            }
            if (Mathf.Abs(transform.position.y - Hook.transform.position.y) < 0.1f)
            {
                transform.position = new Vector2(Hook.position.x, Hook.position.y);
                RemoveCollision();
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, Hook.position, snapSpeed * Time.deltaTime);
            }
        }
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(transform.position.x, mousePosition.y);
        }
    }

    void MoveTowardsHook()
    {
        float step = speed * Time.deltaTime;
        Vector3 hookPosition = nearestHook.transform.position;
        moveDirection = (hookPosition - transform.position).normalized;

        transform.position = Vector2.MoveTowards(transform.position, hookPosition, step);

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        angle += transform.localScale.x < 0 ? 0 : 180; 

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        Quaternion smoothRotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 100) 
        {
            transform.rotation = smoothRotation;
        }
    }

    void MoveOffScreen()
    {
        float direction = transform.localScale.x > 0 ? -1 : 1;

        moveDirection = new Vector2(speed * direction, 0);
        transform.position += new Vector3(moveDirection.x, moveDirection.y, 0) * Time.deltaTime;
    }

    GameObject FindNearestHook()
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hook") && !isDragging)
        {
            transform.SetParent(collision.transform);
            caught = true;
        }
    }
    
    void RemoveCollision()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    void DetachFromHook()
    {
        transform.SetParent(null);
        if (Hook != null)
        {
            Hook.gameObject.GetComponent<Hook>().RecastHook();
        }
    }
    
    private void OnMouseDown()
    {
        StartDragging();
    }
    void StartDragging()
    {
        isDragging = true;
        caught = false;
        DetachFromHook();
    }

    private void OnMouseUp()
    {
        isDragging = false;
        mainCollider.enabled = true;
    }
}