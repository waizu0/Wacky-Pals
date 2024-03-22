using UnityEngine;

/// <summary>
/// Controla a movimentação do jogador em primeira pessoa, incluindo corrida, abaixar-se e a gestão de stamina.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class FirstPersonMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [Tooltip("Velocidade de movimento do jogador.")]
    public float movementSpeed = 5.0f;
    [Tooltip("Multiplicador de velocidade ao correr.")]
    public float runSpeedMultiplier = 1.65f;
    [Tooltip("Quantidade máxima de stamina.")]
    public float maxStamina = 100f;
    [Tooltip("Quantidade de stamina consumida por segundo ao correr.")]
    public float staminaConsumptionRate = 20f;
    [Tooltip("Quantidade de stamina recuperada por segundo após parar de correr.")]
    public float staminaRecoveryRate = 10f;
    [Tooltip("Tempo em segundos antes da stamina começar a se recuperar após parar de correr.")]
    public float staminaRecoveryDelay = 2f;
    [Tooltip("Altura da câmera quando o jogador está em pé.")]
    public float standingCameraHeight;
    [Tooltip("Altura da câmera quando o jogador está abaixado.")]
    public float crouchingCameraHeight;
    [Tooltip("Velocidade da transição entre em pé e abaixado.")]
    public float crouchTransitionSpeed = 5f;

    [Header("Configurações de Rotação")]
    [Tooltip("Sensibilidade do mouse na horizontal.")]
    public float mouseSensitivityX = 2.0f;
    [Tooltip("Sensibilidade do mouse na vertical.")]
    public float mouseSensitivityY = 2.0f;
    [Tooltip("Limita a rotação vertical para evitar giros completos.")]
    public float verticalRotationLimit = 80.0f;

    private CharacterController characterController;
    private Transform cameraTransform;
    private float verticalRotation = 0;
    private float currentStamina;
    private float timeSinceLastRun;
    private bool isCrouching = false;
    private bool isRunning = false;
    private float currentCameraHeight;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = GetComponentInChildren<Camera>().transform;
        currentStamina = maxStamina;
        Cursor.lockState = CursorLockMode.Locked;
        standingCameraHeight = cameraTransform.localPosition.y;
    }

    void Update()
    {
        MovePlayer();
        RecoverStamina();
        HandleCrouching();
        UpdateCameraHeight();
    }

    void MovePlayer()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0 && !isCrouching;
        float speed = isRunning ? movementSpeed * runSpeedMultiplier : movementSpeed;

        if (isRunning)
        {
            ConsumeStamina(Time.deltaTime * staminaConsumptionRate);
            timeSinceLastRun = 0;
        }
        else
        {
            timeSinceLastRun += Time.deltaTime;
        }

        float horizontal = Input.GetAxis("Horizontal") * speed;
        float vertical = Input.GetAxis("Vertical") * speed;
        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void ConsumeStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    void RecoverStamina()
    {
        if (timeSinceLastRun > staminaRecoveryDelay && !isRunning) // Correção aqui: 'isRunning' agora é acessível
        {
            currentStamina += Time.deltaTime * staminaRecoveryRate;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }

    void HandleCrouching()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, isCrouching ? crouchingCameraHeight : standingCameraHeight, cameraTransform.localPosition.z);
        }
    }

    void UpdateCameraHeight()
    {
        float targetHeight = isCrouching ? crouchingCameraHeight : standingCameraHeight;
        // Interpola suavemente a altura atual da câmera até a altura alvo.
        currentCameraHeight = Mathf.Lerp(currentCameraHeight, targetHeight, Time.deltaTime * crouchTransitionSpeed);
        cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, currentCameraHeight, cameraTransform.localPosition.z);
    }

}
