using UnityEngine;

/// <summary>
/// Controla a movimenta��o do jogador em primeira pessoa, incluindo corrida, abaixar-se e a gest�o de stamina.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class FirstPersonMovement : MonoBehaviour
{
    [Header("Configura��es de Movimento")]
    [Tooltip("Velocidade de movimento do jogador.")]
    public float movementSpeed = 5.0f;
    [Tooltip("Multiplicador de velocidade ao correr.")]
    public float runSpeedMultiplier = 1.65f;
    [Tooltip("Quantidade m�xima de stamina.")]
    public float maxStamina = 100f;
    [Tooltip("Quantidade de stamina consumida por segundo ao correr.")]
    public float staminaConsumptionRate = 20f;
    [Tooltip("Quantidade de stamina recuperada por segundo ap�s parar de correr.")]
    public float staminaRecoveryRate = 10f;
    [Tooltip("Tempo em segundos antes da stamina come�ar a se recuperar ap�s parar de correr.")]
    public float staminaRecoveryDelay = 2f;
    [Tooltip("Altura da c�mera quando o jogador est� em p�.")]
    public float standingCameraHeight;
    [Tooltip("Altura da c�mera quando o jogador est� abaixado.")]
    public float crouchingCameraHeight;
    [Tooltip("Velocidade da transi��o entre em p� e abaixado.")]
    public float crouchTransitionSpeed = 5f;

    [Header("Configura��es de Rota��o")]
    [Tooltip("Sensibilidade do mouse na horizontal.")]
    public float mouseSensitivityX = 2.0f;
    [Tooltip("Sensibilidade do mouse na vertical.")]
    public float mouseSensitivityY = 2.0f;
    [Tooltip("Limita a rota��o vertical para evitar giros completos.")]
    public float verticalRotationLimit = 80.0f;

    [Header("Configura��es de Espiar")]
    [Tooltip("Dist�ncia lateral para espiar.")]
    public float peekDistance = 0.5f;
    [Tooltip("Velocidade da transi��o de espiar.")]
    public float peekSpeed = 5f;

    [Header("Uff Refer�ncias")]
    [Tooltip("Refer�ncia ao script de controle da c�mera que cont�m os m�todos de balan�o.")]
    public CameraController cameraController;
    [Tooltip("Refer�ncia ao Animator das m�os.")]
    public Animator handAnimator;

    private CharacterController characterController;
    private Transform cameraTransform;
    private float currentStamina;
    private float timeSinceLastRun;
    private bool isCrouching = false;
    private bool isRunning = false;
    private float currentCameraHeight;
    private float peekSide = 0f; // -1 para esquerda, 1 para direita, 0 para centro
    private bool isMoving = false;


    public bool IsRunning()
    {
        return Input.GetKey(KeyCode.LeftShift) && currentStamina > 0 && !isCrouching;
    }

    public bool IsCrouching { get { return isCrouching; } }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = GetComponentInChildren<Camera>().transform;
        currentStamina = maxStamina;
        standingCameraHeight = cameraTransform.localPosition.y;
    }

    void Update()
    {
        RecoverStamina();
        HandleCrouching();
        UpdateCameraHeight();
        HandlePeek();
        HandleBobbing();
        AdjustHandAnimationSpeed();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        isRunning = IsRunning();
        float speed = movementSpeed;

        if (isRunning)
        {
            speed *= runSpeedMultiplier;
            ConsumeStamina(Time.deltaTime * staminaConsumptionRate);
            timeSinceLastRun = 0;
            if (cameraController) cameraController.RunBob();
        }
        else if (isCrouching)
        {
            speed *= 0.5f;
            if (cameraController) cameraController.CrouchBob();
        }
        else
        {
            if (characterController.velocity.magnitude > 0.1f && cameraController) // Verifica se o personagem est� se movendo
            {
                cameraController.WalkBob();
            }
            timeSinceLastRun += Time.deltaTime;
        }

        float horizontal = Input.GetAxis("Horizontal") * speed;
        float vertical = Input.GetAxis("Vertical") * speed;
        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        characterController.Move(moveDirection * Time.deltaTime);
        isMoving = moveDirection.magnitude > 0; // Atualiza isMoving com base na magnitude do movimento

    }

    void ConsumeStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    void RecoverStamina()
    {
        if (timeSinceLastRun > staminaRecoveryDelay && !isRunning)
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
        }
    }

    void UpdateCameraHeight()
    {
        float targetHeight = isCrouching ? crouchingCameraHeight : standingCameraHeight;
        // Interpola suavemente a altura atual da c�mera at� a altura alvo.
        currentCameraHeight = Mathf.Lerp(currentCameraHeight, targetHeight, Time.deltaTime * crouchTransitionSpeed);
        cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, currentCameraHeight, cameraTransform.localPosition.z);
    }

    void HandlePeek()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            peekSide = -1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            peekSide = 1;
        }
        else
        {
            peekSide = 0;
        }

        float targetPeekPosition = peekDistance * peekSide;
        Vector3 cameraPosition = cameraTransform.localPosition;
        cameraPosition.x = Mathf.Lerp(cameraPosition.x, targetPeekPosition, Time.deltaTime * peekSpeed);
        cameraTransform.localPosition = cameraPosition;
    }

    void HandleBobbing()
    {
        if (!cameraController) return;

        if (isMoving)
        {
            if (isRunning) cameraController.RunBob();
            else if (isCrouching) cameraController.CrouchBob();
            else cameraController.WalkBob();
        }
        else
        {
            Debug.Log("stop bobbing");
            cameraController.StopBob(); // Chama StopBob quando n�o est� se movendo
        }
    }

    /// <summary>
    /// Ajusta a velocidade da anima��o das m�os com base no estado de movimento do jogador.
    /// </summary>
    void AdjustHandAnimationSpeed()
    {
        if (handAnimator == null) return;

        // Se estiver correndo, define o par�metro Speed como 6. Caso contr�rio, volta ao padr�o (1).
        handAnimator.SetFloat("Speed", isRunning ? 8.5f : 1f);
    }
}
