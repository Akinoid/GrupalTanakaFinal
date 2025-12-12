using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
public abstract class BaseCharacter : MonoBehaviour, IPosses
{
    protected Rigidbody rb;
    protected bool isPossessed = false;
    private string thisScene;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        enabled = false;
        thisScene = SceneManager.GetActiveScene().name;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        if (other.CompareTag("lava"))
        {
            HandleLavaCollision(other.gameObject);
        }
    }

    protected virtual void HandleLavaCollision(GameObject lava)
    {
        Kill();
    }

    protected virtual void Kill()
    {
        if (isPossessed)
        {
            OnUnpossessed();
        }

        Destroy(gameObject);
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

    protected virtual void OnDead()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(thisScene);
    }
}
