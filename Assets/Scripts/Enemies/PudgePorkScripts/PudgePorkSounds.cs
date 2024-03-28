using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla a reprodu��o de sons para o Pudge Pork, incluindo sons de passos ao andar.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PudgePorkSounds : MonoBehaviour
{
    [Header("Configura��es de Som")]
    [Tooltip("Lista de AudioClips representando diferentes sons de passos.")]
    [SerializeField] private List<AudioClip> stepSounds = new List<AudioClip>();

    [Tooltip("Refer�ncia ao AudioSource para reproduzir os sons.")]
    public AudioSource audioSource; // AudioSource p�blico para ser acessado e configurado no editor da Unity.

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
    /// Verifica o movimento do personagem para tocar sons de passos conforme necess�rio.
    /// </summary>
    void CheckMovement()
    {
        // Obt�m a refer�ncia ao NavMeshAgent do personagem.
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        // Verifica se o personagem est� se movendo.
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
    /// Reproduz um som de passo aleat�rio da lista de sons de passos.
    /// </summary>
    void PlayStepSound()
    {
        if (stepSounds.Count > 0)
        {
            // Escolhe um AudioClip aleat�rio da lista de sons de passos.
            int index = Random.Range(0, stepSounds.Count);
            AudioClip clip = stepSounds[index];

            // Reproduz o AudioClip selecionado.
            audioSource.PlayOneShot(clip);

            // Aguarda um tempo antes de permitir que outro som de passo seja reproduzido.
            // Isso evita a reprodu��o cont�nua e sobreposta de sons de passos.
            StartCoroutine(WaitForNextStep(clip.length * 2));
        }
    }

    /// <summary>
    /// Coroutine para aguardar um intervalo de tempo antes de permitir a reprodu��o do pr�ximo som de passo.
    /// </summary>
    /// <param name="duration">Dura��o da espera em segundos.</param>
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
