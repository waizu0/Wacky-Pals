using UnityEngine;

/// <summary>
/// Controla a recarga da bateria ao entrar em uma área específica.
/// </summary>
public class Battery : MonoBehaviour
{
    [Header("Configuração do Jogador")]
    [SerializeField, Tooltip("Referência ao gerenciador da lanterna do jogador.")]
    private PlayerFlashlightManager flashlightManager;

    /// <summary>
    /// Método chamado quando um objeto entra no trigger.
    /// </summary>
    /// <param name="other">O colisor que entrou no trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que entrou no trigger é o jogador.
        if (other.CompareTag("Player") && flashlightManager != null)
        {
            Debug.Log("colidiu");
            // Chama o método para recarregar a bateria da lanterna.
            flashlightManager.RechargeBattery();

            this.gameObject.SetActive(false);
        }
    }
}
