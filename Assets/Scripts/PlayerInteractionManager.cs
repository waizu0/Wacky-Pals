using UnityEngine;

/// <summary>
/// Gerencia a interação do jogador com objetos que podem ser interagidos no jogo.
/// Ignora o próprio jogador e seus filhos durante a detecção de objetos interagíveis.
/// </summary>
public class PlayerInteractionManager : MonoBehaviour
{
    [Header("Configurações de Interação")]
    [Tooltip("Distância máxima para a interação.")]
    public float interactionDistance = 5f;

    [Tooltip("Referência para o indicador visual da interação.")]
    public GameObject interactionCue;

    [Header("Configurações de Ignorar")]
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
