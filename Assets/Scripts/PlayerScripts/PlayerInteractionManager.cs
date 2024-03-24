using UnityEngine;
using UnityEngine.UI; // Importando para acessar o componente Image
using System.Collections.Generic; // Necess�rio para usar List<>

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

    [Header("Sprites de Dicas de Intera��o")]
    [Tooltip("Lista de sprites para diferentes objetos interag�veis.")]
    [SerializeField]
    private List<Sprite> interactionSprites;

    [Header("Tags de Objetos Interag�veis")]
    [Tooltip("Lista de tags correspondentes aos sprites de intera��o.")]
    [SerializeField]
    private List<string> interactableTags;

    private Camera playerCamera;
    private float sqrInteractionDistance;
    private Image interactionImage; // Componente Image do indicador de intera��o

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
                    return; // Encontrou um objeto interag�vel, ent�o saia do m�todo
                }
            }
        }

        interactionCue.SetActive(false);
    }
}
