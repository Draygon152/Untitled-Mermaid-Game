using UnityEngine;

public class PlanktonBehavior : MonoBehaviour, IEnemy
{
    public bool hasDetectedByPlayer { get => Detected; set => Detected = value; }
    public GameObject currentGameObject { get => this.gameObject;}
    public Rigidbody2D currentRigidbody2D { get => this.rigidbody2D; }

    private bool Detected = false;
    [SerializeField] private BoxCollider2D collider2D = null;
    [SerializeField] private Rigidbody2D rigidbody2D = null;
    [SerializeField] private GameObject childSpriteObject = null;

    [Header("Spawn Timing")]
    [SerializeField]
    [Tooltip("Random Max switch Time")]
    protected float switchStatusCounterMinTime = 5f;

    [Tooltip("Random Min switch Time")]
    [SerializeField]
    protected float switchStatusCounterMaxTime = 20f;
    private float switchStatusCounter = 0f;

    [Header("Plankton Props")]
    public float floatingSpeed;
    public float floatingMagnitude = 1f;

    public float rotationSpeed;

    public float AliveTime = 10;
    float randomRotation;
    Vector3 randomDirection;



    private void Start()
    {
        collider2D = transform.GetComponent<BoxCollider2D>();
        if(!rigidbody2D){
            rigidbody2D = transform.GetComponent<Rigidbody2D>();
        }
        //random rotation
        randomRotation = Random.Range(0, 360);
        childSpriteObject.transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);

        Invoke("DeactivateSelf", AliveTime);
    }

    private void Update(){
        transform.Translate(Vector2.left * floatingSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        //random rotation
        // randomRotation = Random.Range(0, 360);
        // if (Detected){
        //     switchStatusCounter = Random.Range(switchStatusCounterMinTime, switchStatusCounterMaxTime);
        //     return;
        // } 

        // MoveToRandomLocation();

        
    }

    private void HandleRotationTowardTarget()
    {
        // Rotate the object towards the target direction
        float angleRadians = Mathf.Atan2(randomDirection.y, randomDirection.x);
        float angleDegrees = angleRadians * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angleDegrees);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
    }

    private void MoveToRandomLocation()
    {
        switchStatusCounter -= Time.deltaTime;
        if(switchStatusCounter < 0f){
            MoveInRandomDirection();
            switchStatusCounter = Random.Range(switchStatusCounterMinTime, switchStatusCounterMaxTime);
        }
    }

    private void MoveInRandomDirection()
    {
        // Generate a random direction
        randomDirection = Random.insideUnitSphere.normalized;
        randomDirection = new Vector2(randomDirection.x * floatingMagnitude, randomDirection.y * floatingMagnitude);

        HandleRotationTowardTarget();

        rigidbody2D.velocity = randomDirection * floatingSpeed;
    }

    private void DeactivateSelf()
    {
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