using UnityEngine;

/// <summary>
/// Controla a recarga da bateria ao entrar em uma �rea espec�fica.
/// </summary>
public class Battery : MonoBehaviour
{
    [Header("Configura��o do Jogador")]
    [SerializeField, Tooltip("Refer�ncia ao gerenciador da lanterna do jogador.")]
    private PlayerFlashlightManager flashlightManager;

    /// <summary>
    /// M�todo chamado quando um objeto entra no trigger.
    /// </summary>
    /// <param name="other">O colisor que entrou no trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que entrou no trigger � o jogador.
        if (other.CompareTag("Player") && flashlightManager != null)
        {
            Debug.Log("colidiu");
            // Chama o m�todo para recarregar a bateria da lanterna.
            flashlightManager.RechargeBattery();

            this.gameObject.SetActive(false);
        }
    }
}
