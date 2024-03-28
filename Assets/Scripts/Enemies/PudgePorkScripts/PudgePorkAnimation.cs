using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controla as animações do Pudge Pork, alternando entre caminhar e ficar parado.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class PudgePorkAnimation : MonoBehaviour
{
    private NavMeshAgent agent;
    public Animator animator;
    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Determina se o personagem está andando com base na magnitude da velocidade.
        bool isMoving = agent.velocity.magnitude > 0.1f; // Assume movimento se a velocidade for maior que um limiar mínimo.
        animator.SetBool(IsWalkingHash, isMoving);
    }

    /// <summary>
    /// Ativa ou desativa a animação de encurvamento.
    /// </summary>
    /// <param name="isBending">Se verdadeiro, ativa a animação de encurvar.</param>
    public void SetBending(bool isBending)
    {
        animator.SetBool("isBending", isBending);
    }

}
