using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

/// <summary>
/// Controla o comportamento de um inimigo que patrulha entre pontos predefinidos e responde a sons detectados.
/// A patrulha ocorre entre uma lista de pontos, e o inimigo permanece parado por um tempo aleatório (entre 3 a 7 segundos) em cada ponto antes de continuar para o próximo.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class PudgePorkMain : MonoBehaviour
{
    [Header("Configurações de Patrulha")]
    [Tooltip("Lista de pontos para a patrulha.")]
    [SerializeField] private List<Transform> patrolPoints;

    [Tooltip("O raio máximo de audição do inimigo.")]
    [SerializeField] private float hearingRadius = 20f;

    [Tooltip("O tempo sem ouvir sons para retornar à patrulha.")]
    [SerializeField] private float timeToReturnToPatrol = 7f;

    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private bool isPatrolling = true;
    private float timeSinceLastSoundHeard;
    private float waitTimeAtPoint;
    private float waitTimer = 0f; // Temporizador para controlar o tempo de espera no ponto de patrulha atual

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SoundManager.Instance.OnSoundEmitted += HearSound;

        SetNextPatrolPoint();
    }

    void Update()
    {
        if (isPatrolling)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                if (waitTimer >= waitTimeAtPoint)
                {
                    SetNextPatrolPoint();
                }
                else
                {
                    waitTimer += Time.deltaTime;
                }
            }
        }
        else
        {
            timeSinceLastSoundHeard += Time.deltaTime;
            if (timeSinceLastSoundHeard >= timeToReturnToPatrol)
            {
                isPatrolling = true;
                SetNextPatrolPoint();
            }
        }
    }

    /// <summary>
    /// Define o próximo ponto de patrulha e inicia o temporizador de espera.
    /// </summary>
    private void SetNextPatrolPoint()
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        waitTimeAtPoint = Random.Range(3f, 7f);
        waitTimer = 0f;
    }

    public void HearSound(Vector3 soundLocation, float soundVolume)
    {
        if (Vector3.Distance(transform.position, soundLocation) <= hearingRadius)
        {
            isPatrolling = false;
            timeSinceLastSoundHeard = 0f;
            agent.SetDestination(soundLocation);
        }
    }

    void OnDestroy()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.OnSoundEmitted -= HearSound;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hearingRadius);
    }
}
