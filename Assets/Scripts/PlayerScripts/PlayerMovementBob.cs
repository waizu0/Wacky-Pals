using UnityEngine;

/// <summary>
/// Aplica uma oscilação na rotação da câmera ao andar ou correr para simular o movimento do corpo.
/// </summary>
[RequireComponent(typeof(FirstPersonMovement))]
public class PlayerMovementBob : MonoBehaviour
{
    [Header("Configurações de Oscilação")]
    [Tooltip("Intensidade da oscilação da rotação ao andar.")]
    public float walkBobbingAmount = 0.02f;
    [Tooltip("Intensidade da oscilação da rotação ao correr.")]
    public float runBobbingAmount = 0.05f;
    [Tooltip("Velocidade da oscilação da rotação.")]
    public float bobbingSpeed = 14f;

    private float timer = 0;
    private FirstPersonMovement movementScript;
    private float initialRotationX;
    private float initialRotationY;

    void Start()
    {
        movementScript = GetComponent<FirstPersonMovement>();
        initialRotationX = transform.localEulerAngles.x;
        initialRotationY = transform.localEulerAngles.y;
    }

    void Update()
    {
        ApplyBobEffect();
    }

    /// <summary>
    /// Aplica o efeito de oscilação na rotação baseado no movimento do jogador.
    /// </summary>
    void ApplyBobEffect()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
        {
            // Calcula a oscilação baseada no tempo
            timer += Time.deltaTime * bobbingSpeed;
            float waveSlice = Mathf.Sin(timer);

            // Determina a quantidade de oscilação baseada no estado de movimento (andando ou correndo)
            float bobbingAmount = movementScript.IsRunning() ? runBobbingAmount : walkBobbingAmount;

            // Aplica a oscilação na rotação nos eixos X e Y
            float newRotationX = initialRotationX + waveSlice * bobbingAmount;
            float newRotationY = initialRotationY + waveSlice * bobbingAmount;

            // Atualiza a rotação da câmera
            transform.localEulerAngles = new Vector3(newRotationX, newRotationY, transform.localEulerAngles.z);
        }
        else
        {
            // Reseta a rotação se o jogador não estiver se movendo
            timer = 0;
            transform.localEulerAngles = new Vector3(initialRotationX, initialRotationY, transform.localEulerAngles.z);
        }
    }
}
