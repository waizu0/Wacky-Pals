using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla a reprodução de sons para o Pudge Pork, incluindo sons de passos ao andar.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PudgePorkSounds : MonoBehaviour
{
    [Header("Configurações de Som")]
    [Tooltip("Lista de AudioClips representando diferentes sons de passos.")]
    [SerializeField] private List<AudioClip> stepSounds = new List<AudioClip>();

    [Tooltip("Referência ao AudioSource para reproduzir os sons.")]
    public AudioSource audioSource; // AudioSource público para ser acessado e configurado no editor da Unity.

    private bool isWalking = false;

    // Start is called before the first frame update
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovement();
    }

    /// <summary>
    /// Verifica o movimento do personagem para tocar sons de passos conforme necessário.
    /// </summary>
    void CheckMovement()
    {
        // Obtém a referência ao NavMeshAgent do personagem.
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        // Verifica se o personagem está se movendo.
        if (agent.velocity.magnitude > 0.1f && !isWalking)
        {
            isWalking = true;
            PlayStepSound();
        }
        else if (agent.velocity.magnitude <= 0.1f)
        {
            isWalking = false;
        }
    }

    /// <summary>
    /// Reproduz um som de passo aleatório da lista de sons de passos.
    /// </summary>
    void PlayStepSound()
    {
        if (stepSounds.Count > 0)
        {
            // Escolhe um AudioClip aleatório da lista de sons de passos.
            int index = Random.Range(0, stepSounds.Count);
            AudioClip clip = stepSounds[index];

            // Reproduz o AudioClip selecionado.
            audioSource.PlayOneShot(clip);

            // Aguarda um tempo antes de permitir que outro som de passo seja reproduzido.
            // Isso evita a reprodução contínua e sobreposta de sons de passos.
            StartCoroutine(WaitForNextStep(clip.length * 2));
        }
    }

    /// <summary>
    /// Coroutine para aguardar um intervalo de tempo antes de permitir a reprodução do próximo som de passo.
    /// </summary>
    /// <param name="duration">Duração da espera em segundos.</param>
    /// <returns></returns>
    IEnumerator WaitForNextStep(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (isWalking)
        {
            PlayStepSound();
        }
    }
}
