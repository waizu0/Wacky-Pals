using UnityEngine;

/// <summary>
/// Detecta quando o Pudge Pork está passando por uma porta e ativa ou desativa um estado específico.
/// </summary>
public class PudgePorkDoorDetection : MonoBehaviour
{
    [Header("Configurações de Detecção de Porta")]
    [Tooltip("Tag usada para identificar os objetos de porta.")]
    [SerializeField] private string doorTag = "Door";

    // Referência ao componente que controlará a animação de encurvar.
    private PudgePorkAnimation pudgePorkAnimation;

    private void Start()
    {
        // Obtem o componente PudgePorkAnimation no mesmo GameObject.
        pudgePorkAnimation = GetComponent<PudgePorkAnimation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que colidiu tem a tag de porta.
        if (other.CompareTag(doorTag))
        {
            // Ativa o estado de encurvar.
            pudgePorkAnimation.SetBending(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verifica se o objeto que colidiu tem a tag de porta.
        if (other.CompareTag(doorTag))
        {
            // Desativa o estado de encurvar.
            pudgePorkAnimation.SetBending(false);
        }
    }
}
