using UnityEngine;
using UnityEngine.UI; // Necessário para UI.
using System.Collections.Generic; // Necessário para List<>.

/// <summary>
/// Gerencia a interação do jogador com objetos interagíveis no jogo.
/// Evita a necessidade de múltiplas verificações de tag e GetComponent, utilizando uma abordagem baseada em classe abstrata.
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

    private Camera playerCamera; // Referência para a câmera do jogador.
    private float sqrInteractionDistance; // Distância de interação ao quadrado para otimizar cálculos.
    private Image interactionImage; // Componente Image do indicador de interação.

    void Start()
    {
        playerCamera = Camera.main; // Alocando a câmera principal do jogo.
        interactionCue.SetActive(false); // Desativando a indicação visual de interação por padrão.
        sqrInteractionDistance = interactionDistance * interactionDistance; // Pré-calculando a distância ao quadrado.
        interactionImage = interactionCue.GetComponent<Image>(); // Alocando o componente Image.
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

        // Realiza um raycast para detectar objetos interagíveis.
        bool hitDetected = Physics.Raycast(rayOrigin, rayDirection, out hit, interactionDistance, ~ignoreLayer);

        if (hitDetected && Vector3.SqrMagnitude(hit.point - rayOrigin) <= sqrInteractionDistance)
        {
            InteractableObject interactableObject = hit.collider.GetComponent<InteractableObject>();

            if (interactableObject != null)
            {
                // Ativa o indicador visual e ajusta o sprite conforme necessário aqui.
                interactionCue.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    interactableObject.Interact(); // Chama o método Interact do objeto interagível.
                }
            }
            else
            {
                // Se nenhum objeto interagível foi encontrado, desativa o indicador visual.
                interactionCue.SetActive(false);
            }
        }
        else
        {
            // Se o raycast não detectar nada, desativa o indicador visual.
            interactionCue.SetActive(false);
        }
    }
}
