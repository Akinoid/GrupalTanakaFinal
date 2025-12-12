using UnityEngine;
using Cinemachine;

[AddComponentMenu("Camera/Third Person Camera Controller")]
public class ThirdPersonCameraController : MonoBehaviour
{
    [Tooltip("Referencia al FreeLook virtual camera (arrastrar en inspector)")]
    public CinemachineFreeLook freeLook;

    [Tooltip("Transform del jugador que debe rotar según la cámara (opcional)")]
    public Transform player;

    [Tooltip("Si true, gira el jugador hacia la dirección de la cámara suavemente cuando haya input")]
    public bool rotatePlayerWithCamera = true;

    [Tooltip("Velocidad de rotación del jugador")]
    public float rotationSpeed = 10f;

    [Tooltip("Bloquear cursor al iniciar (Lock) y liberarlo con ESC)")]
    public bool lockCursorOnStart = true;

    void Start()
    {
        if (freeLook == null) Debug.LogWarning("ThirdPersonCameraController: asigna un Cinemachine FreeLook en el inspector.");

        if (lockCursorOnStart)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0) && lockCursorOnStart)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Opcional: girar el jugador hacia la cámara (solo en Y)
        if (rotatePlayerWithCamera && player != null && freeLook != null)
        {
            // Solo girar si hay input de movimiento
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            if (Mathf.Abs(h) > 0.01f || Mathf.Abs(v) > 0.01f)
            {
                Vector3 camForward = freeLook.transform.forward;
                camForward.y = 0f;
                if (camForward.sqrMagnitude > 0.001f)
                {
                    Quaternion target = Quaternion.LookRotation(camForward);
                    player.rotation = Quaternion.Slerp(player.rotation, target, rotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}
