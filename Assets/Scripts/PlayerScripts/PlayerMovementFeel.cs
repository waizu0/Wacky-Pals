using UnityEngine;

/// <summary>
/// Adiciona um efeito de balanço à câmera com base no movimento do jogador para melhorar a sensação de movimento.
/// </summary>
public class PlayerMovementFeel : MonoBehaviour
{
    [Header("Configurações de Balanço")]
    [Tooltip("Intensidade do balanço ao andar.")]
    public float walkBobbingSpeed = 14f;
    [Tooltip("Intensidade do balanço ao correr.")]
    public float runBobbingSpeed = 18f;
    [Tooltip("Intensidade do balanço ao estar agachado.")]
    public float crouchBobbingSpeed = 10f;
    [Tooltip("Quantidade de balanço vertical ao andar.")]
    public float walkBobbingAmount = 0.05f;
    [Tooltip("Quantidade de balanço vertical ao correr.")]
    public float runBobbingAmount = 0.1f;
    [Tooltip("Quantidade de balanço vertical ao estar agachado.")]
    public float crouchBobbingAmount = 0.025f;

    private float defaultYPos = 0;
    private float timer = 0;
    private CharacterController characterController;
    private FirstPersonMovement playerMovement;

    void Start()
    {
        characterController = GetComponentInParent<CharacterController>();
        playerMovement = GetComponentInParent<FirstPersonMovement>();
        defaultYPos = transform.localPosition.y;
    }

    void Update()
    {
        BobHead();
    }

    void BobHead()
    {
        if (Mathf.Abs(characterController.velocity.x) > 0.1f || Mathf.Abs(characterController.velocity.z) > 0.1f)
        {
            // Jogador está se movendo
            timer += Time.deltaTime * GetBobbingSpeed();
            float newY = defaultYPos + Mathf.Sin(timer) * GetBobbingAmount();
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
        else
        {
            // Jogador está parado
            timer = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, defaultYPos, Time.deltaTime * walkBobbingSpeed), transform.localPosition.z);
        }
    }

    float GetBobbingSpeed()
    {
        if (playerMovement.IsCrouching)
        {
            return crouchBobbingSpeed;
        }
        else if (playerMovement.IsRunning())
        {
            return runBobbingSpeed;
        }
        return walkBobbingSpeed;
    }

    float GetBobbingAmount()
    {
        if (playerMovement.IsCrouching)
        {
            return crouchBobbingAmount;
        }
        else if (playerMovement.IsRunning())
        {
            return runBobbingAmount;
        }
        return walkBobbingAmount;
    }
}
