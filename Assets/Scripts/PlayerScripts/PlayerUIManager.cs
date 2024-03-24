using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gerencia a UI do jogador, atualizando a representação visual da stamina e da bateria,
/// assim como a cor com base na quantidade de stamina.
/// </summary>
public class PlayerUIManager : MonoBehaviour
{
    [Header("Configurações da UI de Stamina")]
    [Tooltip("Imagem que representa a stamina do jogador.")]
    [SerializeField] private Image staminaImage;
    [Tooltip("Cor da stamina quando está cheia.")]
    [SerializeField] private Color fullStaminaColor = Color.green;
    [Tooltip("Cor da stamina quando está baixa.")]
    [SerializeField] private Color lowStaminaColor = Color.red;
    [Tooltip("Referência ao script de movimento do jogador para acessar os valores de stamina.")]
    [SerializeField] private FirstPersonMovement playerMovement;

    [Header("Configurações da UI de Bateria")]
    [Tooltip("Imagem que representa a bateria da lanterna.")]
    [SerializeField] private Image batteryImage;
    [Tooltip("Referência ao gerenciador da lanterna para acessar os valores da bateria.")]
    [SerializeField] private PlayerFlashlightManager flashlightManager;

    private void Update()
    {
        UpdateStaminaUI();
        UpdateBatteryUI();
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

    /// <summary>
    /// Atualiza a UI da bateria, ajustando o preenchimento da imagem com base na porcentagem atual da bateria.
    /// </summary>
    private void UpdateBatteryUI()
    {
        if (flashlightManager == null || batteryImage == null) return;

        // Calcula a porcentagem atual da bateria
        float batteryPercent = flashlightManager.BatteryPercentage / 100f; // Assumindo que BatteryPercentage retorna um valor de 0 a 100

        // Atualiza o preenchimento da imagem de bateria
        batteryImage.fillAmount = batteryPercent;
    }
}
