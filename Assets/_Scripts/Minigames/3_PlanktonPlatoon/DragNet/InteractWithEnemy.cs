using UnityEngine;

public class InteractWithEnemy : MonoBehaviour
{
    [SerializeField] private Collider2D interactCollider = null;



    private void Start()
    {
        if (!interactCollider)
        {
            interactCollider = GetComponent<Collider2D>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<IEnemy>(out IEnemy enemy))
        {
            collision.gameObject.SetActive(false);
            EventManager.instance.Notify(EventManager.EventTypes.PlanktonCollected);
        }
    }
}