using UnityEngine;

public class ReverbAreaController : MonoBehaviour
{
    private AudioReverbZone reverbZone;

    void Start()
    {
        // Tenta obter o componente AudioReverbZone associado a este GameObject
        reverbZone = GetComponent<AudioReverbZone>();

        // Certifique-se de desativar o reverb zone inicialmente, se desejado
        if (reverbZone != null) reverbZone.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o jogador entrou na zona
        if (other.CompareTag("Player") && reverbZone != null)
        {
            reverbZone.enabled = true; // Ativa o AudioReverbZone
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verifica se o jogador saiu da zona
        if (other.CompareTag("Player") && reverbZone != null)
        {
            reverbZone.enabled = false; // Desativa o AudioReverbZone
        }
    }
}
