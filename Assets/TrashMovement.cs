using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashMovement : MonoBehaviour
{
    public float speed = 1f;
    private GameObject nearestHook;
    public bool caught = false;
    public Transform Hook;
    public float snapSpeed = 5f;
    private bool isDragging = false;
    private Collider2D mainCollider;
    private Collider2D triggerCollider;

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
            Debug.LogError("Required colliders not found on trash GameObject.");
        }
        if (transform.position.x > 10)
        {
            Vector3 newScale = transform.localScale;
            newScale.x = -newScale.x;
            transform.localScale = newScale;
        }
    }

    void Update()
    {
        if (!caught && !isDragging)
        {
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

    void MoveOffScreen()
    {
        float direction = transform.localScale.x > 0 ? 1 : -1;

        Vector2 moveDirection = new Vector2(speed * direction, 0);
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