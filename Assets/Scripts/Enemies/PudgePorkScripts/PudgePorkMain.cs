using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

/// <summary>
/// Controla o comportamento de um inimigo que patrulha entre pontos predefinidos e responde a sons detectados.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class PudgePorkMain : MonoBehaviour
{
    [Header("Configurações de Patrulha")]
    [Tooltip("Lista de pontos para a patrulha.")]
    [SerializeField]
    private List<Transform> patrolPoints;

    [Tooltip("O raio máximo de audição do inimigo.")]
    [SerializeField]
    private float hearingRadius = 20f;

    [Tooltip("O tempo sem ouvir sons para retornar à patrulha.")]
    [SerializeField]
    private float timeToReturnToPatrol = 7f;

    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private bool isPatrolling = true;
    private float timeSinceLastSoundHeard;

    /// <summary>
    /// Inicializa o agente de navegação e registra o método HearSound para eventos de som emitido.
    /// </summary>
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SoundManager.Instance.OnSoundEmitted += HearSound;

        if (patrolPoints.Count > 0)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    /// <summary>
    /// Atualiza o comportamento de patrulha e resposta a sons.
    /// </summary>
    void Update()
    {
        PatrolRoutine();

        if (!isPatrolling)
        {
            timeSinceLastSoundHeard += Time.deltaTime;

            if (timeSinceLastSoundHeard >= Random.Range(7f, 10f))
            {
                isPatrolling = true;
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            }
        }
    }

    /// <summary>
    /// Executa a rotina de patrulha.
    /// </summary>
    void PatrolRoutine()
    {
        if (isPatrolling && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    /// <summary>
    /// Responde a um som detectado ajustando o destino do agente de acordo.
    /// </summary>
    /// <param name="soundLocation">A localização do som detectado.</param>
    /// <param name="soundVolume">O volume do som detectado.</param>
    public void HearSound(Vector3 soundLocation, float soundVolume)
    {
        float distanceToSound = Vector3.Distance(transform.position, soundLocation);

        if (distanceToSound <= hearingRadius)
        {
            isPatrolling = false;
            timeSinceLastSoundHeard = 0f;
            float distanceFactor = Mathf.Lerp(1f, 0.5f, distanceToSound / hearingRadius);
            Vector3 direction = (soundLocation - transform.position).normalized;
            Vector3 targetPosition = transform.position + direction * distanceToSound * distanceFactor;
            agent.SetDestination(targetPosition);
        }
    }

    /// <summary>
    /// Desregistra o método HearSound dos eventos de som emitido ao destruir o objeto.
    /// </summary>
    void OnDestroy()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.OnSoundEmitted -= HearSound;
        }
    }

    /// <summary>
    /// Desenha uma esfera vermelha para representar o raio de audição do inimigo no Editor.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hearingRadius);
    }
}
