using UnityEngine;

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
    private bool doorOpen = false; // Estado da porta (aberta ou fechada).
    public float defaultVolume = 1.0f; // Volume padrão para tocar os sons.

    public bool debug;

    void Start()
    {
        // Inicializa os componentes necessários, buscando-os nos objetos apropriados.
        doorAnimation = doorChild.GetComponent<Animation>();
    }

    public override void Interact()
    {
        ToggleDoor();
    }

    // Como teste, ao apertar C, fecha/abre a porta.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && debug)
        {
            ToggleDoor();
        }
    }

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

    private void OpenDoor()
    {
        doorAnimation.Play("Open"); // Nome da animação de abertura.
        SoundManager.Instance.EmitSound(transform.position, defaultVolume, openSound);
        doorOpen = true;
    }

    private void CloseDoor()
    {
        doorAnimation.Play("Close"); // Nome da animação de fechamento.
        SoundManager.Instance.EmitSound(transform.position, defaultVolume, closeSound);
        doorOpen = false;
    }
}
