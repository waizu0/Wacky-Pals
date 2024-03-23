using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Monitora e exibe o desempenho do jogo, incluindo uso de mem�ria e FPS (m�nimo, m�dio, m�ximo e atual).
/// </summary>
public class PerformanceManager : MonoBehaviour
{
    [Header("Configura��es de Exibi��o")]
    [Tooltip("Texto para exibir as m�tricas de desempenho.")]
    public TextMeshProUGUI displayText;

    [Header("Configura��es de Desempenho")]
    [Tooltip("Intervalo de atualiza��o das m�tricas em segundos.")]
    public float updateInterval = 0.5f;

    private float deltaTime = 0.0f;
    private float minFps = float.MaxValue;
    private float maxFps = 0;
    private float totalFps = 0;
    private int frameCount = 0;
    private float currentFps = 0; // Armazena o FPS atual para exibi��o

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

        // Atualiza o FPS m�nimo, m�ximo e total
        if (currentFps < minFps)
        {
            minFps = currentFps;
        }

        maxFps = Mathf.Max(maxFps, currentFps);
        totalFps += currentFps;
        frameCount++;
    }

    /// <summary>
    /// Atualiza periodicamente as m�tricas de desempenho e as exibe.
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
    /// Exibe as m�tricas de desempenho no texto UI, incluindo o FPS atual.
    /// </summary>
    /// <param name="averageFps">FPS m�dio.</param>
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
    /// Reinicia as m�tricas de FPS para a pr�xima janela de c�lculo.
    /// </summary>
    private void ResetFpsMetrics()
    {
        // Mant�m o valor m�nimo entre atualiza��es, pois estamos interessados no m�nimo global
        totalFps = 0;
        frameCount = 0;
    }
}
