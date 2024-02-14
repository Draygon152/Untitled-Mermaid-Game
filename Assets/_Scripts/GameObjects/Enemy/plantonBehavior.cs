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
    public float floatingMagnitude = 1f;

    public float rotationSpeed;

    public float AliveTime = 10;
    float randomRotation;
    Vector3 randomDirection;


    // Start is called before the first frame update
    void Start()
    {
        collider2D = transform.GetComponent<BoxCollider2D>();
        if(!rigidbody2D){
            rigidbody2D = transform.GetComponent<Rigidbody2D>();
        }
        //random rotation
        randomRotation = Random.Range(0, 360);
        this.transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);

        Invoke("DeactivateSelf", AliveTime);
    }

    // Update is called once per frame
    void Update()
    {
        //random rotation
        randomRotation = Random.Range(0, 360);
        if(Detected){
            switchStatusCounter = Random.Range(switchStatusCounterMinTime, switchStatusCounterMaxTime);
            return;
        } 

        moveToRandomLocation();
        handleRotationTowardTarget();

    }


    public void handleRotationTowardTarget(){
        // Rotate the object towards the target direction
        float angleRadians = Mathf.Atan2(randomDirection.y, randomDirection.x);
        float angleDegrees = angleRadians * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angleDegrees);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }


    public void moveToRandomLocation(){
        switchStatusCounter -= Time.deltaTime;
        if(switchStatusCounter < 0f){
            MoveInRandomDirection();
            switchStatusCounter = Random.Range(switchStatusCounterMinTime, switchStatusCounterMaxTime);
        }
    }

    public void MoveInRandomDirection(){
        // Generate a random direction
        randomDirection = Random.insideUnitSphere.normalized;
        randomDirection = new Vector2(randomDirection.x  * floatingMagnitude, randomDirection.y  * floatingMagnitude);
        rigidbody2D.velocity = randomDirection * floatingSpeed;
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
        //going diection
        Gizmos.DrawLine(transform.position, transform.position + randomDirection);
    }
}
