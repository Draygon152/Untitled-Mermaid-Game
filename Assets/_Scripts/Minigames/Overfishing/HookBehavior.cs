using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBehavior : MonoBehaviour
{
    [SerializeField] private float descentSpeed = 5f;
    public bool caughtFish = false;

    void Update()
    {
        if (caughtFish)
        {
            transform.Translate(Vector2.up * descentSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.down * descentSpeed * Time.deltaTime);
        }
        if (transform.position.y > 13)
        {
            RemoveHook();
        }
    }

    public void RemoveHook()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bound"))
        {
            RemoveHook();
        }
    }
}