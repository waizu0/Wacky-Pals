using UnityEngine;

/// <summary>
/// Gerencia a abertura e fechamento de uma porta, estendendo a classe base InteractableObject.
/// Define comportamentos específicos de interação com a porta, como reprodução de animações e efeitos sonoros.
/// </summary>
[RequireComponent(typeof(Animation), typeof(AudioSource))]
public class DoorInteraction : InteractableObject
{
    [Header("Configurações da Porta")]
    [Tooltip("Referência para o objeto da porta que possui a animação.")]
    [SerializeField] private GameObject doorChild;

    [Header("Efeitos Sonoros da Porta")]
    [Tooltip("Efeito sonoro de abrir a porta.")]
    [SerializeField] private AudioClip openSound;
    [Tooltip("Efeito sonoro de fechar a porta.")]
    [SerializeField] private AudioClip closeSound;

    private Animation doorAnimation; // Componente Animation do objeto da porta.
    private AudioSource audioSource; // Componente AudioSource para tocar os efeitos sonoros.
    private bool doorOpen = false; // Estado da porta (aberta ou fechada).

    void Start()
    {
        // Inicializa os componentes necessários, buscando-os nos objetos apropriados.
        doorAnimation = doorChild.GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Método de interação que alterna o estado da porta, implementado da classe base InteractableObject.
    /// Verifica o estado atual da porta e a alterna entre aberta e fechada.
    /// </summary>
    public override void Interact()
    {
        ToggleDoor();
    }

    /// <summary>
    /// Alterna o estado da porta, verificando se ela está atualmente aberta ou fechada e acionando a animação apropriada.
    /// Impede a interação se uma animação já estiver sendo reproduzida, para evitar sobreposições ou interrupções indesejadas.
    /// </summary>
    private void ToggleDoor()
    {
        if (doorAnimation.isPlaying) return; // Impede a interação durante a reprodução de uma animação.

        if (!doorOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    /// <summary>
    /// Executa a animação e o som de abertura da porta.
    /// Define a animação "Open" para ser reproduzida e o efeito sonoro de abrir, acionando ambos.
    /// </summary>
    private void OpenDoor()
    {
        doorAnimation.Play("Open"); // Nome da animação de abertura.
        audioSource.clip = openSound;
        audioSource.Play();
        doorOpen = true;
    }

    /// <summary>
    /// Executa a animação e o som de fechamento da porta.
    /// Define a animação "Close" para ser reproduzida e o efeito sonoro de fechar, acionando ambos.
    /// </summary>
    private void CloseDoor()
    {
        doorAnimation.Play("Close"); // Nome da animação de fechamento.
        audioSource.clip = closeSound;
        audioSource.Play();
        doorOpen = false;
    }
}
