using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseController : MonoBehaviour
{
    public float moveSpeed = 5f;

    [SerializeField] private PlayerInputSystem playerInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(playerInput.GetMouseAxisVector());
        mousePosition.x = transform.position.x; // Restrict movement along the x-axis
        mousePosition.z = 0f;

        Vector3 movementDirection = (mousePosition - transform.position).normalized;
        transform.position += movementDirection * moveSpeed * Time.deltaTime;
    }
}
