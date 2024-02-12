using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class plantonBehavior : MonoBehaviour, IEnemy
{
    public bool hasDetectedByPlayer { get => Detected; set => Detected = value; }
    public GameObject currentGameObject { get => this.gameObject;}
    public Rigidbody2D currentRigidbody2D { get => this.rigidbody2D; }

    private bool Detected = false;
    [SerializeField] private BoxCollider2D collider2D;
    [SerializeField] private Rigidbody2D rigidbody2D;

    [Header("Spawn Timing")]
    [SerializeField]
    [Tooltip("Random Max switch Time")]
    protected float switchStatusCounterMinTime = 5f;

    [Tooltip("Random Min switch Time")]
    [SerializeField]
    protected float switchStatusCounterMaxTime = 20f;
    private float switchStatusCounter = 0f;

    [Header("Planton Props")]
    public float floatingSpeed;

    public float AliveTime = 10;


    // Start is called before the first frame update
    void Start()
    {
        collider2D = transform.GetComponent<BoxCollider2D>();
        if(!rigidbody2D){
            rigidbody2D = transform.GetComponent<Rigidbody2D>();
        }
        Debug.Log("yes");

        Invoke("DeactivateSelf", AliveTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(Detected) return;

        switchStatusCounter -= Time.deltaTime;
        if(switchStatusCounter < 0f){
            MoveInRandomDirection();
            switchStatusCounter = Random.Range(switchStatusCounterMinTime, switchStatusCounterMaxTime);
        }
    }

    public void MoveInRandomDirection(){
        // Generate a random direction
        Vector3 randomDirection = Random.insideUnitSphere.normalized;
        rigidbody2D.velocity = new Vector2(randomDirection.x, randomDirection.y) * floatingSpeed;
    }

    public void DeactivateSelf(){
        this.gameObject.SetActive(false);
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
