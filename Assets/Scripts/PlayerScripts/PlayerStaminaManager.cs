using UnityEngine;

/// <summary>
/// Gerencia a stamina do jogador, permitindo correr com um aumento de velocidade e consumindo stamina.
/// </summary>
public class PlayerStaminaManager : MonoBehaviour
{
    [Header("Configura��es de Stamina")]
    [Tooltip("Stamina m�xima do jogador.")]
    public float maxStamina = 100f;
    [Tooltip("Quantidade de stamina que se recupera por segundo.")]
    public float staminaRecoveryRate = 5f;
    [Tooltip("Quantidade de stamina consumida por segundo ao correr.")]
    public float staminaConsumptionRate = 10f;
    [Tooltip("Tempo em segundos antes da recupera��o de stamina come�ar ap�s parar de correr.")]
    public float recoveryDelay = 2f;

    [Header("Configura��es de Corrida")]
    [Tooltip("Multiplicador de velocidade ao correr.")]
    public float runSpeedMultiplier = 1.65f;

    private float currentStamina;
    private float lastRunTime;
    private bool isRunning;

    // Refer�ncia para o script de movimento (substitua "FirstPersonMovement" pelo nome do seu script de movimento)
    private FirstPersonMovement movementScript;

    void Start()
    {
        currentStamina = maxStamina;
        movementScript = GetComponent<FirstPersonMovement>(); // Certifique-se de que este � o nome correto do seu script de movimento
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
    /// Come�a a correr, aumentando a velocidade do jogador.
    /// </summary>
    void StartRunning()
    {
        isRunning = true;
        movementScript.movementSpeed *= runSpeedMultiplier; // Ajuste este acesso conforme a vari�vel do seu script de movimento
    }

    /// <summary>
    /// Para de correr, voltando � velocidade normal.
    /// </summary>
    void StopRunning()
    {
        if (isRunning)
        {
            movementScript.movementSpeed /= runSpeedMultiplier; // Ajuste este acesso conforme a vari�vel do seu script de movimento
            lastRunTime = Time.time;
        }
        isRunning = false;
    }

    /// <summary>
    /// Consome stamina enquanto o jogador est� correndo.
    /// </summary>
    void ConsumeStamina()
    {
        currentStamina -= staminaConsumptionRate * Time.deltaTime;
        currentStamina = Mathf.Max(currentStamina, 0);
    }

    /// <summary>
    /// Recupera a stamina do jogador lentamente ap�s parar de correr.
    /// </summary>
    void RecoverStamina()
    {
        currentStamina += staminaRecoveryRate * Time.deltaTime;
        currentStamina = Mathf.Min(currentStamina, maxStamina);
    }
}
