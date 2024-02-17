using UnityEngine;

public class HookBehavior : MonoBehaviour
{
    [SerializeField] private float descentSpeed = 20f;
    [SerializeField] private Hook hook = null;
    private bool _caughtFish = false;
    public bool caughtFish => _caughtFish;
    private bool hitBottom = false;



    private void Start()
    {
        hook.Init(this);
    }

    private void FixedUpdate()
    {
        if (_caughtFish || hitBottom)
        {
            transform.Translate(Vector2.up * descentSpeed * Time.fixedDeltaTime);
        }

        else
        {
            transform.Translate(Vector2.down * descentSpeed * Time.fixedDeltaTime);
        }
    }

    public void CatchFish()
    {
        _caughtFish = true;
    }

    public void HitBottom()
    {
        hitBottom = true;
    }

    public void RemoveHook()
    {
        gameObject.SetActive(false);
        _caughtFish = false;
        hitBottom = false;
        hook.ResetHook();
    }
}