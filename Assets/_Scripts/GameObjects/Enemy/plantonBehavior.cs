using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour, IEnemy
{
    public bool hasDetectedByPlayer { get => Detected; set => Detected = value; }
    public GameObject currentGameObject { get => this.gameObject;}
    public Rigidbody2D currentRigidbody2D { get => this.rigidbody2D; }

    public bool Detected = false;
    [SerializeField] private BoxCollider2D collider2D;
    [SerializeField] private Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        collider2D = transform.GetComponent<BoxCollider2D>();
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        hasDetectedByPlayer = false;
    }


    public void OnDrawGizmos()
    {
        //got detected
        if(Detected){
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(collider2D.bounds.center, collider2D.bounds.size);
        }
    }
}
