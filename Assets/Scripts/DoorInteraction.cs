using UnityEngine;

/// <summary>
/// Permite ao jogador interagir com a porta, arrastando com o mouse para abrir ou fechar.
/// Observação: Este script deve ser anexado ao GameObject pivot da porta.
/// </summary>
public class DoorDragInteraction : MonoBehaviour
{
    [Header("Configurações de Interação com a Porta")]
    [Tooltip("Velocidade com que a porta segue o arrasto do mouse.")]
    public float dragSpeed = 100f;
    [Tooltip("Força de amortecimento aplicada ao movimento da porta.")]
    public float damping = 5f;

    private Transform playerCamera;
    private bool isDragging = false;
    private float angleDragStart;
    private float angleDoorStart;
    private Vector3 dragDirection;


    private void Start()
    {
        playerCamera = Camera.main.transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && RaycastDoor())
        {
            StartDragging();
        }
        else if (Input.GetMouseButtonUp(0) && isDragging)
        {
            StopDragging();
        }

        if (isDragging)
        {
            DragDoor();
        }
    }

    bool RaycastDoor()
    {
        RaycastHit hit;
        Ray ray = playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.collider.transform == transform)
            {
                return true;
            }
        }
        return false;
    }

    void StartDragging()
    {
        isDragging = true;
        angleDragStart = GetAngleToCursor();
        angleDoorStart = transform.localEulerAngles.y;
        dragDirection = Vector3.Cross(playerCamera.forward, transform.up).normalized;
    }

    void DragDoor()
    {
        float angleCurrent = GetAngleToCursor();
        float angleDelta = angleCurrent - angleDragStart;
        float targetAngle = angleDoorStart + angleDelta * dragSpeed;

        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * damping);
    }

    void StopDragging()
    {
        isDragging = false;
    }

    float GetAngleToCursor()
    {
        Ray ray = playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(-dragDirection, transform.position);
        float enter;
        plane.Raycast(ray, out enter);
        Vector3 intersection = ray.GetPoint(enter);
        Vector3 direction = intersection - transform.position;
        return Vector3.SignedAngle(dragDirection, direction, transform.up);
    }
}
