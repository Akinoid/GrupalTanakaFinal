using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
public class GhostController : MonoBehaviour
{
    public float ghostSpeed = 6f;
    public float detectionRadius = 1.2f;

    private Rigidbody rb;
    private IPosses currentPossessed = null;

    private Vector3 respawnOffset = new Vector3(1.2f, 0, 0);
    private bool isPossessing = false;
    private Vector3 intialLocalScale;
    private Renderer[] renderers;
    [SerializeField] CinemachineCamera cinemachineCamera;
    Collider h;
    private string thisScene;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        renderers = GetComponentsInChildren<Renderer>();
        thisScene = SceneManager.GetActiveScene().name;
        intialLocalScale = transform.localScale;
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
                transform.localScale = intialLocalScale;
            this.h = h;
            cinemachineCamera.Follow = h.gameObject.transform;
            /*BaseCharacter c = h.GetComponent<BaseCharacter>();
            c.cinemachineCamera.Priority = 1;
            cinemachineCamera.Priority = 0;*/
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
        transform.localScale = intialLocalScale;

        SetGhostVisible(true);
        cinemachineCamera.Follow = gameObject.transform;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        if (other.CompareTag("lava"))
        {
            HandleLavaCollision(other.gameObject);
        }
    }

    protected void HandleLavaCollision(GameObject lava)
    {
        Kill();
    }

    protected void Kill()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(thisScene);
    }
}
