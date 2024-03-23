using UnityEngine;

/// <summary>
/// Controla a rotação da câmera e do personagem em jogos de primeira pessoa.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Configurações de Rotação")]
    [Tooltip("Determina se a rotação está habilitada.")]
    public bool rotate = true;
    [Tooltip("Sensibilidade do mouse na horizontal.")]
    public float mouseSensitivityX = 2.0f;
    [Tooltip("Sensibilidade do mouse na vertical.")]
    public float mouseSensitivityY = 2.0f;
    [Tooltip("Limita a rotação vertical para evitar giros completos.")]
    public float verticalRotationLimit = 80.0f;


    private float verticalRotation = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (rotate)
        {
            RotateView();
        }
    }

    /// <summary>
    /// Rotaciona a visão do jogador baseada nas entradas do mouse.
    /// </summary>
    void RotateView()
    {
        // Rotação horizontal
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivityX;
        transform.parent.Rotate(0, horizontalRotation, 0);

        // Rotação vertical
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivityY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);
        transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}
