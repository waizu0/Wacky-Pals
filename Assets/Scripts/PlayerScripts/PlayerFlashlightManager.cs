using UnityEngine;
using System.Collections;

/// <summary>
/// Controla a funcionalidade de um objeto lanterna.
/// </summary>
public class PlayerFlashlightManager : MonoBehaviour
{
    [SerializeField, Tooltip("O componente de luz da lanterna.")]
    private Light flashlightLight;

    [Header("Configurações da Bateria")]
    [SerializeField, Tooltip("Capacidade máxima da bateria em segundos.")]
    private float batteryLifeTime = 120f;

    [SerializeField, Tooltip("Porcentagem da bateria na qual a luz começa a piscar.")]
    private float criticalBatteryPercentage = 10f;

    public float batteryPercentage = 100f;
    private bool isBlinking = false;
    private bool isOn = true;

    private void Start()
    {
        if (!flashlightLight) flashlightLight = GetComponent<Light>();
        flashlightLight.enabled = isOn;
        if (isOn) StartCoroutine(BatteryDrain());
    }

    private void Update()
    {
        ToggleFlashlight();
        if (batteryPercentage <= criticalBatteryPercentage && batteryPercentage > 0 && isOn && !isBlinking)
        {
            StartCoroutine(RandomBlinking());
        }
        else if (batteryPercentage <= 0)
        {
            flashlightLight.enabled = false;
        }
    }

    private void ToggleFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOn = !isOn;
            flashlightLight.enabled = isOn;

            if (isOn)
            {
                StartCoroutine(BatteryDrain());
            }
            else
            {
                StopAllCoroutines();
                isBlinking = false;
                flashlightLight.enabled = false;
            }
        }
    }

    private IEnumerator RandomBlinking()
    {
        isBlinking = true;
        while (batteryPercentage <= criticalBatteryPercentage && isOn)
        {
            flashlightLight.enabled = !flashlightLight.enabled;
            yield return new WaitForSeconds(Random.Range(0.1f, 1.0f)); // Intervalos aleatórios entre 0.1 e 1 segundo
        }
        isBlinking = false;
    }

    private IEnumerator BatteryDrain()
    {
        while (batteryPercentage > 0 && isOn)
        {
            yield return new WaitForSeconds(1);
            batteryPercentage -= 100f / batteryLifeTime;
            if (batteryPercentage <= 0)
            {
                flashlightLight.enabled = false;
                isOn = false;
            }
        }
    }

    public void RechargeBattery()
    {
        batteryPercentage = 100f;
        isBlinking = false;
        StopAllCoroutines(); // Para todas as corrotinas para reiniciar o estado da lanterna corretamente.

        if (isOn)
        {
            // Somente reinicia a corrotina de drenagem de bateria se a lanterna estiver ligada.
            StartCoroutine(BatteryDrain());
        }
    }

}
