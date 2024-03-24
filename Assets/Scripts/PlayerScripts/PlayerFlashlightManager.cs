using UnityEngine;
using System.Collections;

/// <summary>
/// Controla a funcionalidade de um objeto lanterna, incluindo anima��es para ligar e desligar.
/// </summary>
public class PlayerFlashlightManager : MonoBehaviour
{
    [SerializeField, Tooltip("O componente de luz da lanterna.")]
    private Light flashlightLight;

    [Header("Configura��es da Bateria")]
    [SerializeField, Tooltip("Capacidade m�xima da bateria em segundos.")]
    private float batteryLifeTime = 120f;

    [SerializeField, Tooltip("Porcentagem da bateria na qual a luz come�a a piscar.")]
    private float criticalBatteryPercentage = 10f;

    [Header("Configura��es de Anima��o")]
    [SerializeField, Tooltip("Animator respons�vel pelas anima��es da m�o.")]
    private Animator handAnimator;

    private float batteryPercentage = 100f;
    private bool isBlinking = false;
    private bool isOn = true;
    private float lastToggleTime = -2f; // Inicializa com um valor que permite a ativa��o imediata no come�o
    private float toggleDelay = .55f; // Tempo necess�rio para esperar antes de poder ligar/desligar novamente

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
            lastToggleTime = Time.time; // Atualiza o tempo da �ltima a��o
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
        yield return null; // Removido o WaitForSeconds para garantir que a mudan�a de estado � imediata mas com controle de tempo para nova ativa��o
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
