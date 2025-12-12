using UnityEngine;
using Cinemachine;


public class CinemachineMouseInput : MonoBehaviour
{
    [Tooltip("Multiplicador para la entrada del mouse")]
    public float sensitivity = 1f;

    private CinemachineCore.AxisInputDelegate previousDelegate;

    void OnEnable()
    {
        previousDelegate = CinemachineCore.GetInputAxis;
        CinemachineCore.GetInputAxis = GetAxis;
    }

    void OnDisable()
    {
        CinemachineCore.GetInputAxis = previousDelegate;
    }

    float GetAxis(string axisName)
    {
        if (axisName == "Mouse X") return Input.GetAxis("Mouse X") * sensitivity;
        if (axisName == "Mouse Y") return Input.GetAxis("Mouse Y") * sensitivity;
        return Input.GetAxis(axisName);
    }
}
