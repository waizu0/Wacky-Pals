using UnityEngine;
using UnityEngine.UI; // Importando para acessar o componente Image
using System.Collections.Generic; // Necessário para usar List<>

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

    [Header("Sprites de Dicas de Interação")]
    [Tooltip("Lista de sprites para diferentes objetos interagíveis.")]
    [SerializeField]
    private List<Sprite> interactionSprites;

    [Header("Tags de Objetos Interagíveis")]
    [Tooltip("Lista de tags correspondentes aos sprites de interação.")]
    [SerializeField]
    private List<string> interactableTags;

    private Camera playerCamera;
    private float sqrInteractionDistance;
    private Image interactionImage; // Componente Image do indicador de interação

    void Start()
    {
        playerCamera = Camera.main;
        interactionCue.SetActive(false);
        sqrInteractionDistance = interactionDistance * interactionDistance;
        interactionImage = interactionCue.GetComponent<Image>(); // Alocando o componente Image
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

        if (hitDetected && Vector3.SqrMagnitude(hit.point - rayOrigin) <= sqrInteractionDistance)
        {
            for (int i = 0; i < interactableTags.Count; i++)
            {
                if (hit.collider.CompareTag(interactableTags[i]))
                {
                    interactionCue.SetActive(true);
                    interactionImage.sprite = interactionSprites[i];
                    return; // Encontrou um objeto interagível, então saia do método
                }
            }
        }

        interactionCue.SetActive(false);
    }
}
