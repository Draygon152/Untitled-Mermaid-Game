using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBehavior : MonoBehaviour
{
    [SerializeField] private float descentSpeed = 5f;
    public bool caughtFish = false;
    private bool hitBottom = false;

    void Update()
    {
        if (caughtFish || hitBottom)
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
        if (transform.position.y < 0)
        {
            hitBottom = true;
        }
    }

    public void RemoveHook()
    {
        gameObject.SetActive(false);
        caughtFish = false;
        hitBottom = false;
        GetComponentInChildren<BoxCollider2D>().enabled = true;
    }
}