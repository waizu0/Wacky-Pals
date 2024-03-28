using UnityEngine;

/// <summary>
/// Gerencia a rea��o do jogador a jumpscares, desativando a movimenta��o, a lanterna e a UI.
/// </summary>
public class PlayerJumpscareScript : MonoBehaviour
{
    [Header("Configura��es de Jumpscare")]
    [Tooltip("Objeto para o jumpscare do tipo PudgePork.")]
    [SerializeField] private GameObject pudgePorkJumpscareObject;
    [Tooltip("Objeto para o jumpscare do tipo LankyLuke.")]
    [SerializeField] private GameObject lankyLukeJumpscareObject;
    [SerializeField] private GameObject body;
    [SerializeField] private CameraShake cameraShake;


    private FirstPersonMovement playerMovement;
    private PlayerFlashlightManager flashlightManager;
    private PlayerUIManager uiManager;
    public CameraController controller;

    void Start()
    {
        // Busca os componentes necess�rios nos objetos do jogador
        playerMovement = GetComponent<FirstPersonMovement>();
        flashlightManager = FindObjectOfType<PlayerFlashlightManager>();
        uiManager = FindObjectOfType<PlayerUIManager>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            TriggerPudgePorkJumpscare();
        }
    }

    /// <summary>
    /// Executa a rea��o ao jumpscare de PudgePork.
    /// </summary>
    public void TriggerPudgePorkJumpscare()
    {
        // Desativa o objeto do jumpscare ap�s ser usado
        if (pudgePorkJumpscareObject) pudgePorkJumpscareObject.SetActive(true);

        ReactToJumpscare();
    }

    /// <summary>
    /// Executa a rea��o ao jumpscare de LankyLuke.
    /// </summary>
    public void TriggerLankyLukeJumpscare()
    {
        // Desativa o objeto do jumpscare ap�s ser usado
        if (lankyLukeJumpscareObject) lankyLukeJumpscareObject.SetActive(false);

        ReactToJumpscare();
    }

    /// <summary>
    /// Define as a��es comuns a serem executadas ap�s um jumpscare.
    /// </summary>
    private void ReactToJumpscare()
    {
        // Desativa a movimenta��o do jogador
        if (playerMovement) playerMovement.enabled = false;

        // Desativa a lanterna
        if (flashlightManager) flashlightManager.enabled = false;

        // Desativa a UI
        if (uiManager) uiManager.enabled = false;

        // Treme a câmera
        if (cameraShake) cameraShake.Shake();

        controller.enabled = false;

        body.SetActive(false);
    }
}
