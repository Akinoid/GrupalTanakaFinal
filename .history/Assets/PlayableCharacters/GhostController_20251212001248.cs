using UnityEngine;

public class GhostController : MonoBehaviour
{
    public float ghostSpeed = 6f;
    public float detectionRadius = 1.2f;

    private Rigidbody rb;
    private IPosses currentPossessed = null;

    private Vector3 respawnOffset = new Vector3(1.2f, 0, 0);
    private bool isPossessing = false;

    private Renderer[] renderers;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void Update()
    {
        if (!isPossessing)
        {
            HandleGhostMovement();
            TryPossess();
        }
        else
        {
            TryUnpossess();
        }
    }

    private void HandleGhostMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0f, v).normalized;

        rb.linearVelocity = dir * ghostSpeed;
    }

    private void TryPossess()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (var h in hits)
        {
            IPosses target = h.GetComponent<IPosses>();
            if (target == null) continue;

            

            currentPossessed = target;

            currentPossessed.OnPossessed();
            currentPossessed.EnableControl();

            isPossessing = true;

            
            rb.linearVelocity = Vector3.zero;
            
                transform.SetParent(h.transform, false);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            

            SetGhostVisible(false);
            break;
        }
    }

    private void TryUnpossess()
    {
        if (!Input.GetKeyDown(KeyCode.Q)) return;

        currentPossessed.DisableControl();
        currentPossessed.OnUnpossessed();

        MonoBehaviour mb = currentPossessed as MonoBehaviour;

        
        transform.SetParent(null);
        if (mb != null)
        {
            transform.position = mb.transform.position + respawnOffset;
        }

        rb.linearVelocity = Vector3.zero;
        

        SetGhostVisible(true);

        currentPossessed = null;
        isPossessing = false;
    }

    private void SetGhostVisible(bool visible)
    {
        foreach (var r in renderers)
        {
            r.enabled = visible;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
