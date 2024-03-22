using UnityEngine;

/// <summary>
/// Gerencia a stamina do jogador, permitindo correr com um aumento de velocidade e consumindo stamina.
/// </summary>
public class PlayerStaminaManager : MonoBehaviour
{
    [Header("Configurações de Stamina")]
    [Tooltip("Stamina máxima do jogador.")]
    public float maxStamina = 100f;
    [Tooltip("Quantidade de stamina que se recupera por segundo.")]
    public float staminaRecoveryRate = 5f;
    [Tooltip("Quantidade de stamina consumida por segundo ao correr.")]
    public float staminaConsumptionRate = 10f;
    [Tooltip("Tempo em segundos antes da recuperação de stamina começar após parar de correr.")]
    public float recoveryDelay = 2f;

    [Header("Configurações de Corrida")]
    [Tooltip("Multiplicador de velocidade ao correr.")]
    public float runSpeedMultiplier = 1.65f;

    private float currentStamina;
    private float lastRunTime;
    private bool isRunning;

    // Referência para o script de movimento (substitua "FirstPersonMovement" pelo nome do seu script de movimento)
    private FirstPersonMovement movementScript;

    void Start()
    {
        currentStamina = maxStamina;
        movementScript = GetComponent<FirstPersonMovement>(); // Certifique-se de que este é o nome correto do seu script de movimento
    }

    void Update()
    {
        HandleRunningInput();

        if (isRunning)
        {
            ConsumeStamina();
        }
        else if (Time.time - lastRunTime >= recoveryDelay)
        {
            RecoverStamina();
        }
    }

    /// <summary>
    /// Trata da entrada do jogador para correr.
    /// </summary>
    void HandleRunningInput()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            StartRunning();
        }
        else
        {
            StopRunning();
        }
    }

    /// <summary>
    /// Começa a correr, aumentando a velocidade do jogador.
    /// </summary>
    void StartRunning()
    {
        isRunning = true;
        movementScript.movementSpeed *= runSpeedMultiplier; // Ajuste este acesso conforme a variável do seu script de movimento
    }

    /// <summary>
    /// Para de correr, voltando à velocidade normal.
    /// </summary>
    void StopRunning()
    {
        if (isRunning)
        {
            movementScript.movementSpeed /= runSpeedMultiplier; // Ajuste este acesso conforme a variável do seu script de movimento
            lastRunTime = Time.time;
        }
        isRunning = false;
    }

    /// <summary>
    /// Consome stamina enquanto o jogador está correndo.
    /// </summary>
    void ConsumeStamina()
    {
        currentStamina -= staminaConsumptionRate * Time.deltaTime;
        currentStamina = Mathf.Max(currentStamina, 0);
    }

    /// <summary>
    /// Recupera a stamina do jogador lentamente após parar de correr.
    /// </summary>
    void RecoverStamina()
    {
        currentStamina += staminaRecoveryRate * Time.deltaTime;
        currentStamina = Mathf.Min(currentStamina, maxStamina);
    }
}
