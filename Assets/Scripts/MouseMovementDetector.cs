using UnityEngine;

/// <summary>
/// Detecta o movimento vertical do mouse e dispara eventos correspondentes, além de registrar os movimentos.
/// </summary>
public class MouseMovementDetector : MonoBehaviour
{
    [Header("Sensibilidade do Movimento Vertical")]
    [Tooltip("Ajusta a sensibilidade do movimento vertical do mouse.")]
    [SerializeField] private float sensitivity = 0.1f;

    private float lastMouseYPosition;

    /// <summary>
    /// Chamado quando o mouse se move para cima.
    /// </summary>
    public event System.Action OnMouseMoveUp;

    /// <summary>
    /// Chamado quando o mouse se move para baixo.
    /// </summary>
    public event System.Action OnMouseMoveDown;

    private void Start()
    {
        lastMouseYPosition = Input.mousePosition.y;
    }

    private void Update()
    {
        DetectMouseMovement();
    }

    /// <summary>
    /// Detecta o movimento vertical do mouse, dispara eventos apropriados e registra o movimento.
    /// </summary>
    private void DetectMouseMovement()
    {
        float mouseYDelta = Input.mousePosition.y - lastMouseYPosition;

        if (Mathf.Abs(mouseYDelta) > sensitivity)
        {
            if (mouseYDelta > 0 && Input.GetKey(KeyCode.Mouse0))
            {
                Debug.Log("Movendo para cima");
                OnMouseMoveUp?.Invoke();
            }
            else if (mouseYDelta < 0 && Input.GetKey(KeyCode.Mouse0))
            {
                Debug.Log("Movendo para baixo");
                OnMouseMoveDown?.Invoke();
            }
        }

        lastMouseYPosition = Input.mousePosition.y;
    }
}
