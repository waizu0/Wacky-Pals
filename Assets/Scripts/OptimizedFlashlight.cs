using UnityEngine;

[RequireComponent(typeof(Light))]
public class SmoothTransitionLightIntensityController : MonoBehaviour
{
    [SerializeField] private float closeIntensity = 0.2f;  // Intensidade quando muito perto
    [SerializeField] private float farIntensity = 5f;      // Intensidade quando distante
    [SerializeField] private float checkInterval = 0.1f;   // Intervalo entre verificações
    [SerializeField] private float detectionDistance = 10f; // Distância máxima de detecção
    [SerializeField] private float smoothStartDistance = 2f; // Distância a partir da qual a luz começa a atenuar
    [SerializeField] private float transitionSpeed = 1f;   // Velocidade de transição da intensidade da luz

    private Light _light;
    private float _targetIntensity;
    private float _nextCheckTime = 0f;

    private void Awake()
    {
        _light = GetComponent<Light>();
        _targetIntensity = _light.intensity;
    }

    private void Update()
    {
        if (Time.time >= _nextCheckTime)
        {
            AdjustTargetIntensity();
            _nextCheckTime = Time.time + checkInterval;
        }

        // Suaviza a transição da intensidade atual para o alvo
        _light.intensity = Mathf.Lerp(_light.intensity, _targetIntensity, Time.deltaTime * transitionSpeed);
    }

    private void AdjustTargetIntensity()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, detectionDistance))
        {
            // Se estiver dentro do intervalo de suavização, interpola a intensidade com base na distância
            if (hit.distance < smoothStartDistance)
            {
                float intensityRatio = hit.distance / smoothStartDistance;
                _targetIntensity = Mathf.Lerp(closeIntensity, farIntensity, intensityRatio);
            }
            else
            {
                _targetIntensity = farIntensity;
            }
        }
        else
        {
            // Se não atingir nada dentro da distância de detecção, ajusta o alvo para a intensidade distante
            _targetIntensity = farIntensity;
        }
    }
}
