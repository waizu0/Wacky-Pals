using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Monitora e exibe o desempenho do jogo, incluindo uso de memória e FPS (mínimo, médio, máximo e atual).
/// </summary>
public class PerformanceManager : MonoBehaviour
{
    [Header("Configurações de Exibição")]
    [Tooltip("Texto para exibir as métricas de desempenho.")]
    public TextMeshProUGUI displayText;

    [Header("Configurações de Desempenho")]
    [Tooltip("Intervalo de atualização das métricas em segundos.")]
    public float updateInterval = 0.5f;

    private float deltaTime = 0.0f;
    private float minFps = float.MaxValue;
    private float maxFps = 0;
    private float totalFps = 0;
    private int frameCount = 0;
    private float currentFps = 0; // Armazena o FPS atual para exibição

    void Start()
    {
        StartCoroutine(UpdatePerformanceMetrics());
    }

    void Update()
    {
        UpdateFps();
    }

    /// <summary>
    /// Atualiza a contagem de FPS e calcula o FPS atual.
    /// </summary>
    private void UpdateFps()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        currentFps = 1.0f / deltaTime;

        // Atualiza o FPS mínimo, máximo e total
        if (currentFps < minFps)
        {
            minFps = currentFps;
        }

        maxFps = Mathf.Max(maxFps, currentFps);
        totalFps += currentFps;
        frameCount++;
    }

    /// <summary>
    /// Atualiza periodicamente as métricas de desempenho e as exibe.
    /// </summary>
    /// <returns>Coroutine.</returns>
    private IEnumerator UpdatePerformanceMetrics()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);
            float averageFps = totalFps / frameCount;
            DisplayMetrics(averageFps);
            ResetFpsMetrics();
        }
    }

    /// <summary>
    /// Exibe as métricas de desempenho no texto UI, incluindo o FPS atual.
    /// </summary>
    /// <param name="averageFps">FPS médio.</param>
    private void DisplayMetrics(float averageFps)
    {
        System.GC.Collect();
        long totalMemory = System.GC.GetTotalMemory(false);
        displayText.text = $"RAM: {totalMemory / 1024 / 1024} MB - " +
                            $"FPS MIN: {minFps:N2} - " +
                            $"FPS MED: {averageFps:N2} - " +
                            $"FPS MAX: {maxFps:N2} - " +
                            $"FPS ATUAL: {currentFps:N2}";
    }

    /// <summary>
    /// Reinicia as métricas de FPS para a próxima janela de cálculo.
    /// </summary>
    private void ResetFpsMetrics()
    {
        // Mantém o valor mínimo entre atualizações, pois estamos interessados no mínimo global
        totalFps = 0;
        frameCount = 0;
    }
}
