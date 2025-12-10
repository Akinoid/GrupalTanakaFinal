using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour , IPosses
{
    protected Rigidbody rb;
    protected bool isPossessed = false;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        enabled = false;
    }

    public virtual void OnPossessed()
    {
        isPossessed = true;

        enabled = true;
    }

    public virtual void OnUnpossessed()
    {
        isPossessed = false;

        if (rb != null)
            rb.linearVelocity = Vector3.zero;

        enabled = false;
    }

    private void Update()
    {
        if (!enabled) return;
        HandleMovement();
        HandleAbilities();
    }
    public virtual void EnableControl() { }

    public virtual void DisableControl()
    {
        if (rb != null)
            rb.linearVelocity = Vector3.zero;
    }

    protected abstract void HandleMovement();
    protected abstract void HandleAbilities();
}
