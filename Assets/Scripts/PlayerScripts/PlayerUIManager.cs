using UnityEngine;
using UnityEngine.UI; // Necess�rio para trabalhar com UI

/// <summary>
/// Gerencia a UI do jogador, atualizando a representa��o visual da stamina e a cor com base na quantidade de stamina.
/// </summary>
public class PlayerUIManager : MonoBehaviour
{
    [Header("Configura��es da UI de Stamina")]
    [Tooltip("Imagem que representa a stamina do jogador.")]
    [SerializeField] private Image staminaImage;
    [Tooltip("Cor da stamina quando est� cheia.")]
    [SerializeField] private Color fullStaminaColor = Color.green;
    [Tooltip("Cor da stamina quando est� baixa.")]
    [SerializeField] private Color lowStaminaColor = Color.red;
    [Tooltip("Refer�ncia ao script de movimento do jogador para acessar os valores de stamina.")]
    [SerializeField] private FirstPersonMovement playerMovement;

    private void Update()
    {
        UpdateStaminaUI();
    }

    /// <summary>
    /// Atualiza a UI da stamina, ajustando o preenchimento e a cor da imagem com base na quantidade atual de stamina do jogador.
    /// </summary>
    private void UpdateStaminaUI()
    {
        if (playerMovement == null || staminaImage == null) return;

        // Calcula a porcentagem atual de stamina
        float staminaPercent = playerMovement.CurrentStamina / playerMovement.MaxStamina;

        // Atualiza o preenchimento da imagem de stamina
        staminaImage.fillAmount = staminaPercent;

        // Interpola a cor da imagem de stamina com base na porcentagem de stamina
        staminaImage.color = Color.Lerp(lowStaminaColor, fullStaminaColor, staminaPercent);
    }
}
