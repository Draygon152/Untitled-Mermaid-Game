using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider = null;
    private HookBehavior _rod = null;
    public HookBehavior rod => _rod;



    public void Init(HookBehavior rodParent)
    {
        _rod = rodParent;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fish") && !_rod.caughtFish)
        {
            FishMovement fish = collision.gameObject.GetComponent<FishMovement>();

            if (!fish.caught)
            {
                fish.OnCaught(transform);

                _rod.CatchFish();
                boxCollider.enabled = false;
            }
        } 

        else if (collision.gameObject.CompareTag("Trash"))
        {
            TrashMovement trash = collision.gameObject.GetComponent<TrashMovement>();

            if (!trash.caught)
            {
                trash.OnCaught(transform);

                _rod.CatchFish();
                boxCollider.enabled = false;
            }
        }

        else if (collision.gameObject.CompareTag("Wall"))
        {
            _rod.HitBottom();
        }
    }

    public void ResetHook()
    {
        boxCollider.enabled = true;
    }

    private void OnBecameInvisible()
    {
        if (_rod.caughtFish)
        {
            _rod.RemoveHook();
        }
    }
}
