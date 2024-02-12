using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithEnemy : MonoBehaviour
{
    // Start is called before the first frame update

    public Collider2D interactCollider;
    void Start()
    {
        if(!interactCollider){
            interactCollider = GetComponent<Collider2D>();
        }
    }


    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.TryGetComponent<IEnemy>(out IEnemy enemy)){
            collision.gameObject.SetActive(false);
        }

    }
}
