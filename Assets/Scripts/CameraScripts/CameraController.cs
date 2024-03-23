using UnityEngine;

/// <summary>
/// Controla a rota��o da c�mera e do personagem em jogos de primeira pessoa,
/// adicionando efeitos de "balan�o" ao caminhar, correr ou abaixar, afetando a rota��o da c�mera.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Configura��es de Rota��o")]
    [Tooltip("Determina se a rota��o est� habilitada.")]
    public bool rotate = true;
    [Tooltip("Sensibilidade do mouse na horizontal.")]
    public float mouseSensitivityX = 2.0f;
    [Tooltip("Sensibilidade do mouse na vertical.")]
    public float mouseSensitivityY = 2.0f;
    [Tooltip("Limita a rota��o vertical para evitar giros completos.")]
    public float verticalRotationLimit = 80.0f;

    [Header("Configura��es de Balan�o ao Movimentar")]
    [Tooltip("Velocidade do balan�o ao caminhar.")]
    public float walkBobSpeed = 14f;
    [Tooltip("Velocidade do balan�o ao correr.")]
    public float runBobSpeed = 18f;
    [Tooltip("Velocidade do balan�o ao abaixar.")]
    public float crouchBobSpeed = 8f;
    [Tooltip("Quantidade de rota��o ao balan�ar.")]
    public float bobAmount = 0.05f;

    private float verticalRotation = 0;
    private float timer = 0;
    private float bobSpeed = 0; // Nova vari�vel para controlar a velocidade do balan�o de forma din�mica
    private float targetBobSpeed = 0; // Velocidade alvo do balan�o, para suavizar a transi��o

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

        // Suaviza��o da velocidade do balan�o para parar lentamente
        bobSpeed = Mathf.Lerp(bobSpeed, targetBobSpeed, Time.deltaTime * 2); // Ajuste o fator 2 para controlar a suavidade
        if (bobSpeed > 0.01f) // Verifica se a velocidade � significativa para aplicar o balan�o
        {
            BobMotion(bobSpeed);
        }
        else
        {
            timer = 0; // Reset do timer para evitar descontinuidade no movimento
        }
    }

    public void WalkBob()
    {
        targetBobSpeed = walkBobSpeed;
    }

    public void RunBob()
    {
        targetBobSpeed = runBobSpeed;
    }

    public void CrouchBob()
    {
        targetBobSpeed = crouchBobSpeed;
    }

    public void StopBob()
    {
        targetBobSpeed = 0; // Define a velocidade alvo como 0 para parar o balan�o
    }

    void BobMotion(float speed)
    {
        timer += Time.deltaTime * speed;
        float bobSwayFactor = Mathf.Sin(timer) * bobAmount;
        transform.localRotation = Quaternion.Euler(verticalRotation + bobSwayFactor, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    void RotateView()
    {
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivityX;
        transform.parent.Rotate(0, horizontalRotation, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivityY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);
        transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

}
