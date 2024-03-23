using UnityEngine;

/// <summary>
/// Controla a rotação da câmera e do personagem em jogos de primeira pessoa,
/// adicionando efeitos de "balanço" ao caminhar, correr ou abaixar, afetando a rotação da câmera.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Configurações de Rotação")]
    [Tooltip("Determina se a rotação está habilitada.")]
    public bool rotate = true;
    [Tooltip("Sensibilidade do mouse na horizontal.")]
    public float mouseSensitivityX = 2.0f;
    [Tooltip("Sensibilidade do mouse na vertical.")]
    public float mouseSensitivityY = 2.0f;
    [Tooltip("Limita a rotação vertical para evitar giros completos.")]
    public float verticalRotationLimit = 80.0f;

    [Header("Configurações de Balanço ao Movimentar")]
    [Tooltip("Velocidade do balanço ao caminhar.")]
    public float walkBobSpeed = 14f;
    [Tooltip("Velocidade do balanço ao correr.")]
    public float runBobSpeed = 18f;
    [Tooltip("Velocidade do balanço ao abaixar.")]
    public float crouchBobSpeed = 8f;
    [Tooltip("Quantidade de rotação ao balançar.")]
    public float bobAmount = 0.05f;

    private float verticalRotation = 0;
    private float timer = 0;
    private float bobSpeed = 0; // Nova variável para controlar a velocidade do balanço de forma dinâmica
    private float targetBobSpeed = 0; // Velocidade alvo do balanço, para suavizar a transição

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

        // Suavização da velocidade do balanço para parar lentamente
        bobSpeed = Mathf.Lerp(bobSpeed, targetBobSpeed, Time.deltaTime * 2); // Ajuste o fator 2 para controlar a suavidade
        if (bobSpeed > 0.01f) // Verifica se a velocidade é significativa para aplicar o balanço
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
        targetBobSpeed = 0; // Define a velocidade alvo como 0 para parar o balanço
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
