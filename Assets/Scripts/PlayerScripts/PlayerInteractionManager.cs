using UnityEngine;
using UnityEngine.UI; // Necess�rio para UI.
using System.Collections.Generic; // Necess�rio para List<>.

/// <summary>
/// Gerencia a intera��o do jogador com objetos interag�veis no jogo.
/// Evita a necessidade de m�ltiplas verifica��es de tag e GetComponent, utilizando uma abordagem baseada em classe abstrata.
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

    private Camera playerCamera; // Refer�ncia para a c�mera do jogador.
    private float sqrInteractionDistance; // Dist�ncia de intera��o ao quadrado para otimizar c�lculos.
    private Image interactionImage; // Componente Image do indicador de intera��o.

    void Start()
    {
        playerCamera = Camera.main; // Alocando a c�mera principal do jogo.
        interactionCue.SetActive(false); // Desativando a indica��o visual de intera��o por padr�o.
        sqrInteractionDistance = interactionDistance * interactionDistance; // Pr�-calculando a dist�ncia ao quadrado.
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

        // Realiza um raycast para detectar objetos interag�veis.
        bool hitDetected = Physics.Raycast(rayOrigin, rayDirection, out hit, interactionDistance, ~ignoreLayer);

        if (hitDetected && Vector3.SqrMagnitude(hit.point - rayOrigin) <= sqrInteractionDistance)
        {
            InteractableObject interactableObject = hit.collider.GetComponent<InteractableObject>();

            if (interactableObject != null)
            {
                // Ativa o indicador visual e ajusta o sprite conforme necess�rio aqui.
                interactionCue.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    interactableObject.Interact(); // Chama o m�todo Interact do objeto interag�vel.
                }
            }
            else
            {
                // Se nenhum objeto interag�vel foi encontrado, desativa o indicador visual.
                interactionCue.SetActive(false);
            }
        }
        else
        {
            // Se o raycast n�o detectar nada, desativa o indicador visual.
            interactionCue.SetActive(false);
        }
    }
}
