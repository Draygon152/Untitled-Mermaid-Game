using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            collision.gameObject.GetComponent<FishMovement>().caught = true;
            collision.gameObject.GetComponent<FishMovement>().Hook = gameObject.GetComponent<Transform>();
            gameObject.GetComponentInParent<HookBehavior>().caughtFish = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
