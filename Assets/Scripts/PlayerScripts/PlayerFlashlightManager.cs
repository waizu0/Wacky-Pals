using UnityEngine;
using System.Collections;

/// <summary>
/// Controla a funcionalidade de um objeto lanterna, incluindo animações para ligar e desligar.
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

    [Header("Configurações de Animação")]
    [SerializeField, Tooltip("Animator responsável pelas animações da mão.")]
    private Animator handAnimator;

    private float batteryPercentage = 100f;
    private bool isBlinking = false;
    private bool isOn = true;
    private float lastToggleTime = -2f; // Inicializa com um valor que permite a ativação imediata no começo
    private float toggleDelay = .55f; // Tempo necessário para esperar antes de poder ligar/desligar novamente

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
            FlashlightOut();
        }
    }

    private void ToggleFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F) && Time.time - lastToggleTime >= toggleDelay)
        {
            lastToggleTime = Time.time; // Atualiza o tempo da última ação
            StartCoroutine(ToggleFlashlightWithDelay());
        }
    }


    private IEnumerator ToggleFlashlightWithDelay()
    {
        isOn = !isOn;
        flashlightLight.enabled = isOn;

        if (isOn)
        {
            handAnimator.Play("FlashlightOn");
            StartCoroutine(BatteryDrain());
        }
        else
        {
            handAnimator.Play("FlashlightOut");
            StopAllCoroutines();
            isBlinking = false;
            flashlightLight.enabled = false;
        }
        yield return null; // Removido o WaitForSeconds para garantir que a mudança de estado é imediata mas com controle de tempo para nova ativação
    }
    private IEnumerator RandomBlinking()
    {
        isBlinking = true;
        while (batteryPercentage <= criticalBatteryPercentage && isOn)
        {
            flashlightLight.enabled = !flashlightLight.enabled;
            yield return new WaitForSeconds(Random.Range(0.1f, 1.0f));
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
                FlashlightOut();
            }
        }
    }

    private void FlashlightOut()
    {
        handAnimator.Play("FlashlightOut");
        flashlightLight.enabled = false;
        isOn = false;
        StopAllCoroutines();
    }

    public void RechargeBattery()
    {
        batteryPercentage = 100f;
        isBlinking = false;
        StopAllCoroutines();

        if (isOn)
        {
            StartCoroutine(BatteryDrain());
        }
    }
}
