using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class ObjectDetection : MonoBehaviour
{

    [Header ("Detection logic")]
    [Tooltip("The objects that we are searching for")]
    public GameObject[] targetObject;
    [Tooltip("The field of view angle of player (in degrees)")]
    public float fieldOfViewAngle = 90;
    [Tooltip("The distance that the agent can see")]
    public float viewDistance = 5f;

    [Tooltip("The detection start location")]
    public Transform detectionStartLocation;

    [Tooltip("Attaching power")]
    public float AttachingPower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // LineOfSight();
    }

    void LineOfSight(){

        //circle detection
        Collider2D[] colliders = Physics2D.OverlapCircleAll(detectionStartLocation.position, viewDistance);
        foreach(Collider2D collider in colliders){
            IEnemy enemyCheck;
            if(collider.gameObject.TryGetComponent<IEnemy>(out enemyCheck)){
                WithinInSight(enemyCheck);
            }
        }
    
    }

    void WithinInSight(IEnemy target){
        Vector2 direction = target.currentGameObject.transform.position - detectionStartLocation.position;
        float angle = Vector2.Angle(direction, detectionStartLocation.transform.forward);


        if(!(direction.magnitude < viewDistance && angle < fieldOfViewAngle * 0.5f)){
            target.hasDetectedByPlayer = false;
            return;
        }

        // Check if the enemy is within the field of view cone
        float dot = Vector2.Dot(direction.normalized, detectionStartLocation.transform.right);

        if(!(dot > Mathf.Cos(fieldOfViewAngle * 0.5f * Mathf.Deg2Rad))){
            target.hasDetectedByPlayer = false;
            return;
        }

        target.hasDetectedByPlayer = true;
        Vector2 towardPlayerDirection = (detectionStartLocation.transform.position - target.currentGameObject.transform.position).normalized;

        if(!target.currentRigidbody2D) return;
        target.currentRigidbody2D.AddForce(towardPlayerDirection * AttachingPower, ForceMode2D.Force);
        
    }

    public void OnDrawGizmos()
    {
        var oldColor = Gizmos.color;
        var color = Color.yellow;
        color.a = 0.1f;
        UnityEditor.Handles.color = color;

        var halfFOV = fieldOfViewAngle * 0.5f;
        var beginDirection = Quaternion.AngleAxis(-halfFOV, Vector3.forward) * transform.right;
        UnityEditor.Handles.DrawSolidArc(detectionStartLocation.transform.position, detectionStartLocation.transform.forward, beginDirection, fieldOfViewAngle, viewDistance);

        UnityEditor.Handles.color = oldColor;
    }

}
