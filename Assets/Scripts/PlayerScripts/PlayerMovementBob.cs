using UnityEngine;

/// <summary>
/// Aplica uma oscila��o na rota��o da c�mera ao andar ou correr para simular o movimento do corpo.
/// </summary>
[RequireComponent(typeof(FirstPersonMovement))]
public class PlayerMovementBob : MonoBehaviour
{
    [Header("Configura��es de Oscila��o")]
    [Tooltip("Intensidade da oscila��o da rota��o ao andar.")]
    public float walkBobbingAmount = 0.02f;
    [Tooltip("Intensidade da oscila��o da rota��o ao correr.")]
    public float runBobbingAmount = 0.05f;
    [Tooltip("Velocidade da oscila��o da rota��o.")]
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
    /// Aplica o efeito de oscila��o na rota��o baseado no movimento do jogador.
    /// </summary>
    void ApplyBobEffect()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
        {
            // Calcula a oscila��o baseada no tempo
            timer += Time.deltaTime * bobbingSpeed;
            float waveSlice = Mathf.Sin(timer);

            // Determina a quantidade de oscila��o baseada no estado de movimento (andando ou correndo)
            float bobbingAmount = movementScript.IsRunning() ? runBobbingAmount : walkBobbingAmount;

            // Aplica a oscila��o na rota��o nos eixos X e Y
            float newRotationX = initialRotationX + waveSlice * bobbingAmount;
            float newRotationY = initialRotationY + waveSlice * bobbingAmount;

            // Atualiza a rota��o da c�mera
            transform.localEulerAngles = new Vector3(newRotationX, newRotationY, transform.localEulerAngles.z);
        }
        else
        {
            // Reseta a rota��o se o jogador n�o estiver se movendo
            timer = 0;
            transform.localEulerAngles = new Vector3(initialRotationX, initialRotationY, transform.localEulerAngles.z);
        }
    }
}
