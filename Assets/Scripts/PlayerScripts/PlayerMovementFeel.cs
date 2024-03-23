using UnityEngine;

/// <summary>
/// Adiciona um efeito de balan�o � c�mera com base no movimento do jogador para melhorar a sensa��o de movimento.
/// </summary>
public class PlayerMovementFeel : MonoBehaviour
{
    [Header("Configura��es de Balan�o")]
    [Tooltip("Intensidade do balan�o ao andar.")]
    public float walkBobbingSpeed = 14f;
    [Tooltip("Intensidade do balan�o ao correr.")]
    public float runBobbingSpeed = 18f;
    [Tooltip("Intensidade do balan�o ao estar agachado.")]
    public float crouchBobbingSpeed = 10f;
    [Tooltip("Quantidade de balan�o vertical ao andar.")]
    public float walkBobbingAmount = 0.05f;
    [Tooltip("Quantidade de balan�o vertical ao correr.")]
    public float runBobbingAmount = 0.1f;
    [Tooltip("Quantidade de balan�o vertical ao estar agachado.")]
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
            // Jogador est� se movendo
            timer += Time.deltaTime * GetBobbingSpeed();
            float newY = defaultYPos + Mathf.Sin(timer) * GetBobbingAmount();
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
        else
        {
            // Jogador est� parado
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
