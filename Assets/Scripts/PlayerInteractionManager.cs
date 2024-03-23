using UnityEngine;

/// <summary>
/// Gerencia a intera��o do jogador com objetos que podem ser interagidos no jogo.
/// Ignora o pr�prio jogador e seus filhos durante a detec��o de objetos interag�veis.
/// </summary>
public class PlayerInteractionManager : MonoBehaviour
{
    [Header("Configura��es de Intera��o")]
    [Tooltip("Dist�ncia m�xima para a intera��o.")]
    public float interactionDistance = 5f;

    [Tooltip("Refer�ncia para o indicador visual da intera��o.")]
    public GameObject interactionCue;

    [Header("Configura��es de Ignorar")]
    [Tooltip("Camadas para ignorar durante o raycast.")]
    public LayerMask ignoreLayer;

    private Camera playerCamera;
    private float sqrInteractionDistance;

    void Start()
    {
        playerCamera = Camera.main;
        interactionCue.SetActive(false);
        sqrInteractionDistance = interactionDistance * interactionDistance;
    }

    void Update()
    {
        CheckForInteractableObject();
    }

    private void CheckForInteractableObject()
    {
        RaycastHit hit;
        Vector3 rayOrigin = playerCamera.transform.position;
        Vector3 rayDirection = playerCamera.transform.forward;
        bool hitDetected = Physics.Raycast(rayOrigin, rayDirection, out hit, interactionDistance, ~ignoreLayer);

        if (hitDetected && hit.collider.CompareTag("Interact") && Vector3.SqrMagnitude(hit.point - rayOrigin) <= sqrInteractionDistance)
        {
            interactionCue.SetActive(true);
        }
        else
        {
            interactionCue.SetActive(false);
        }
    }
}
